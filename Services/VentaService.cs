using Microsoft.EntityFrameworkCore;
using BoletosCore.Data;
using BoletosCore.Models;

namespace BoletosCore.Services
{
    public class VentaService
    {
        private readonly AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Boleto>, string)> RegistrarVenta(Pasajero pasajero, int cantidadBoletos, DateTime fechaViaje)
        {
            var boletos = new List<Boleto>();

            // Guardar o recuperar pasajero
            var pasajeroExistente = await _context.Pasajeros
                .FirstOrDefaultAsync(p => p.Cedula == pasajero.Cedula);
            if (pasajeroExistente == null)
            {
                _context.Pasajeros.Add(pasajero);
                await _context.SaveChangesAsync();
                pasajeroExistente = pasajero;
            }

            // Buscar bus disponible y comparar con cooperativas
            var bus = await _context.Buses
                .Where(b => b.FechaViaje.Date == fechaViaje.Date && b.Capacidad >= 45)
                .Include(b => b.Boletos)
                .FirstOrDefaultAsync();

            if (bus == null)
            {
                bus = new Bus
                {
                    Capacidad = 45,
                    Placa = "BUS-" + DateTime.Now.Ticks,
                    FechaViaje = fechaViaje,
                    Boletos = new List<Boleto>()
                };
                _context.Buses.Add(bus);
                await _context.SaveChangesAsync();
            }

            var asientosOcupados = bus.Boletos.Count;
            var asientosDisponibles = bus.Capacidad - asientosOcupados;

            // Comparar asientos disponibles con capacidad de cooperativas (lógica de ejemplo)
            var cooperativas = await _context.Cooperativas.ToListAsync();
            var cooperativasConCapacidad = cooperativas.Where(c => c.ComisionPorcentaje < 15).ToList(); // Ejemplo de comparación

            if (asientosDisponibles >= cantidadBoletos)
            {
                for (int i = 0; i < cantidadBoletos; i++)
                {
                    var boleto = new Boleto
                    {
                        PasajeroId = pasajeroExistente.Id,
                        BusId = bus.Id,
                        NumeroAsiento = asientosOcupados + i + 1,
                        Destino = "Ibarra",
                        FechaViaje = fechaViaje,
                        Precio = 5.00m,
                        Estado = "Vendido"
                    };
                    boletos.Add(boleto);
                    _context.Boletos.Add(boleto);
                }
                await _context.SaveChangesAsync();
                return (boletos, "Boletos asignados en nuestro bus.");
            }
            else
            {
                var cooperativa = cooperativasConCapacidad.FirstOrDefault();
                if (cooperativa != null)
                {
                    for (int i = 0; i < cantidadBoletos; i++)
                    {
                        var boleto = new Boleto
                        {
                            PasajeroId = pasajeroExistente.Id,
                            BusId = bus.Id,
                            NumeroAsiento = 0,
                            Destino = "Ibarra",
                            FechaViaje = fechaViaje,
                            Precio = 5.00m,
                            Estado = "Redirigido"
                        };
                        _context.Boletos.Add(boleto);
                        await _context.SaveChangesAsync();

                        var boletoRedirigido = new BoletoRedirigido
                        {
                            BoletoId = boleto.Id,
                            CooperativaId = cooperativa.Id,
                            Comision = boleto.Precio * (cooperativa.ComisionPorcentaje / 100),
                            FechaRedireccion = DateTime.Now
                        };
                        _context.BoletosRedirigidos.Add(boletoRedirigido);
                        boletos.Add(boleto);
                    }
                    await _context.SaveChangesAsync();
                    return (boletos, $"Boletos redirigidos a {cooperativa.Nombre}.");
                }
                return (boletos, "No hay cooperativas disponibles para redirigir.");
            }
        }
    }
}