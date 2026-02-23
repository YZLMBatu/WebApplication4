namespace WebApplication4.Models
{
    public class Musteri
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Telefon { get; set; } // İsteğe bağlı
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}