using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 


namespace WebApplication4.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class SatisController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public SatisController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        
        [Authorize(Roles = "Admin,Kasiyer")]
        [HttpPost("SatisYap")]
        public IActionResult SatisYap(int urunId, int adet, string musteriAdi)
        {
            try
            {
               
                var subeId = User.FindFirst("SubeId")?.Value;

                _inventoryService.ProcessSale(urunId, adet, musteriAdi);

                return Ok(new
                {
                    mesaj = "Satış başarıyla işlendi.",
                    islemYapanSube = subeId,
                    detay = $"{musteriAdi} isimli müşterinin memnuniyet anketi 30 dk sonra gönderilecektir."
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { hata = ex.Message });
            }
        }

       
        [Authorize(Roles = "Admin")]
        [HttpDelete("SatisIptal/{id}")]
        public IActionResult SatisIptal(int id)
        {
            
            return Ok(new { mesaj = $"{id} numaralı satış iptal edildi." });
        }
    }
}