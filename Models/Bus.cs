namespace BoletosCore.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public int Capacidad { get; set; }
        public string Placa { get; set; }
        public DateTime FechaViaje { get; set; }
        public List<Boleto> Boletos { get; set; }
    }
}