namespace WebApplication4.Models
{
    public class ReceteDetay
    {
        public int Id { get; set; }
        public int YemekId { get; set; }
        public int HammaddeId { get; set; }
        public Hammadde Hammadde { get; set; }
        public double GerekenMiktar { get; set; }
    }
}