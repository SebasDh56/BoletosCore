namespace BoletosCore.Models
{
    public class Cooperativa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Prioridad { get; set; }
        public decimal ComisionPorcentaje { get; set; }
        public List<BoletoRedirigido> BoletosRedirigidos { get; set; }
    }
}