using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            
            if (login.KullaniciAdi == "batuhan" && login.Sifre == "123456")
            {
                var jwtSettings = _config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, login.KullaniciAdi),
                    new Claim(ClaimTypes.Role, "Admin"), 
                    new Claim("", "1"), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                
                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1), 
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    kullanici = login.KullaniciAdi
                });
            }

            return Unauthorized(new { mesaj = "Kullanıcı adı veya şifre hatalı!" });
        }
    }

    
    public class LoginModel
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
    }
}