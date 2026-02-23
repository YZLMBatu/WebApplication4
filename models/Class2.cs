namespace WebApplication4.Models
{
    public class Hammadde
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public double MevcutStok { get; set; }
        public double KritikSeviye { get; set; }
        public double VarsayilanSiparisMiktari { get; set; }
        public string Birim { get; set; }
        public DateTime SonGuncellenme { get; set; } = DateTime.Now;
    }
}