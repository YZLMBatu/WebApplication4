using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SatisController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public SatisController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpPost("SatisYap")]
    
    public IActionResult SatisYap(int urunId, int adet, string musteriAdi)
    {
        try
        {
            
            _inventoryService.ProcessSale(urunId, adet, musteriAdi);

            return Ok(new
            {
                mesaj = "Satış başarıyla işlendi.",
                detay = $"{musteriAdi} isimli müşteri geçmişini kontrol edniniz"
            });
        }
        catch (System.Exception ex)
        {
            
            return BadRequest(new { hata = ex.Message });
        }
    }
}