namespace BoletosCore.Models
{
    public class Pasajero
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string? Email { get; set; }
        public List<Boleto> Boletos { get; set; }
    }
}