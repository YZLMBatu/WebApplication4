using Microsoft.EntityFrameworkCore;
using WebApplication4.models;
using WebApplication4.Models;

public class InventoryService
{
    private readonly AppDbContext _context;

    public InventoryService(AppDbContext context) { _context = context; }

    
    public void ProcessSale(int productId, int quantity, string musteriAdi)
    {
        try
        {
            var yemek = _context.Yemekler
                .Include(p => p.ReceteDetaylari)
                .ThenInclude(rd => rd.Hammadde)
                .FirstOrDefault(p => p.Id == productId);

            if (yemek == null) throw new Exception("Ürün bulunamadı!");

            var kasa = _context.Kasalar.FirstOrDefault();
            if (kasa == null) throw new Exception("Kasa kaydı bulunamadı!");

            
            var musteri = _context.Musteriler.FirstOrDefault(m => m.Ad.ToLower() == musteriAdi.ToLower());

            if (musteri == null)
            {
                
                musteri = new Musteri { Ad = musteriAdi, KayitTarihi = DateTime.Now };
                _context.Musteriler.Add(musteri);
                _context.SaveChanges(); 
            }

            
            var yeniSiparis = new MusteriSiparis
            {
                MusteriId = musteri.Id,
                YemekId = productId,
                Miktar = quantity,
                SiparisTarihi = DateTime.Now
            };
            _context.MusteriSiparisleri.Add(yeniSiparis);

            
            decimal satisTutari = yemek.Fiyat * quantity;
            kasa.ToplamCiro += satisTutari;

            
            foreach (var detay in yemek.ReceteDetaylari)
            {
                detay.Hammadde.MevcutStok -= (detay.GerekenMiktar * quantity);

                
                decimal hammaddeMaliyeti = (decimal)(detay.GerekenMiktar * quantity) * (decimal)detay.Hammadde.VarsayilanSiparisMiktari;
                kasa.ToplamCiro -= hammaddeMaliyeti;

                if (detay.Hammadde.MevcutStok <= detay.Hammadde.KritikSeviye)
                {
                    TriggerAutoOrder(detay.Hammadde);
                }
            }

            kasa.SonIslemTarihi = DateTime.Now;
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Satış İşlenemedi: {ex.Message}");
        }
    }


    private void TriggerAutoOrder(Hammadde hammadde)
    {
        
        bool hasPendingOrder = _context.OtomatikSiparisler
            .Any(o => o.HammaddeId == hammadde.Id && o.Durum == "Bekliyor");

        if (!hasPendingOrder)
        {
            var siparis = new OtomatikSiparis
            {
                HammaddeId = hammadde.Id,
                Miktar = hammadde.VarsayilanSiparisMiktari,
                Durum = "Bekliyor",
                SiparisTarihi = DateTime.Now
            };
            _context.OtomatikSiparisler.Add(siparis);
        }
    }
}