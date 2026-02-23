namespace WebApplication4.Models
{
    public class Kasa
    {
        public int Id { get; set; }
        public decimal ToplamCiro { get; set; }
        public DateTime SonIslemTarihi { get; set; } = DateTime.Now;
    }
}