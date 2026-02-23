namespace WebApplication4.Models
{
    public class Yemek
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
        public List<ReceteDetay> ReceteDetaylari { get; set; }
    }
}