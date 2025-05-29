namespace BoletosCore.Models
{
    public class BoletoRedirigido
    {
        public int Id { get; set; }
        public int BoletoId { get; set; }
        public Boleto Boleto { get; set; }
        public int CooperativaId { get; set; }
        public Cooperativa Cooperativa { get; set; }
        public decimal Comision { get; set; }
        public DateTime FechaRedireccion { get; set; }
    }
}