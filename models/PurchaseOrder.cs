namespace WebApplication4.Models
{
    public class OtomatikSiparis
    {
        public int Id { get; set; }
        public int HammaddeId { get; set; }
        public Hammadde Hammadde { get; set; }
        public double Miktar { get; set; }
        public string Durum { get; set; } = "Bekliyor";
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;
    }
}