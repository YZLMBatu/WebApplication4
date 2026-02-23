namespace WebApplication4.models
{
    public class MusteriSiparis
    {
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public int YemekId { get; set; }
        
        public int Miktar { get; set; } 
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;

    }
}
