using Microsoft.EntityFrameworkCore;
using WebApplication4.models;
using WebApplication4.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Hammadde> Hammaddeler { get; set; }
    public DbSet<Yemek> Yemekler { get; set; }
    public DbSet<ReceteDetay> ReceteDetaylari { get; set; }
    public DbSet<OtomatikSiparis> OtomatikSiparisler { get; set; }
    public DbSet<Kasa> Kasalar { get; set; }
    public DbSet<HammaddeAlim> HammaddeAlimlar { get; set; }

    public DbSet<MusteriSiparis> MusteriSiparisleri { get; set; }
    public DbSet<Musteri> Musteriler { get; set; }
}