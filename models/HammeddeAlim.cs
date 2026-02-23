namespace WebApplication4.Models
{
    public class HammaddeAlim
    {
        public int Id { get; set; }
        public int HammaddeId { get; set; }
        public Hammadde Hammadde { get; set; }
        public double Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public DateTime AlimTarihi { get; set; } = DateTime.Now;
    }
}