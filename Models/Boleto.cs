namespace BoletosCore.Models
{
    public class Boleto
    {
        public int Id { get; set; }
        public int PasajeroId { get; set; }
        public Pasajero Pasajero { get; set; }
        public int BusId { get; set; }
        public Bus Bus { get; set; }
        public int NumeroAsiento { get; set; }
        public string Destino { get; set; }
        public DateTime FechaViaje { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
        public List<BoletoRedirigido> Redirecciones { get; set; }
    }
}