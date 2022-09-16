using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityTests.Controllers
{
    [ApiController]
    [Route("/api/auth2")]
    public class ApiAuthController2 : ControllerBase
    {

        public ApiAuthController2() { }

        [HttpGet("signin")]
        public async Task<object> ApiSignIn()
        {
            UserInfo user = new UserInfo()
            {
                UserId = 1,
                DisplayName = "FULVIOS",
                UserName = "fulvios",
                Email = "fc@lib.it"
            };

            //create claims details based on the user information
            var claims = new[] {
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("DisplayName", user.DisplayName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx"));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            
            var token = new JwtSecurityToken(
                "JWTAuthenticationServer",
                "JWTServicePostmanClient",
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);
           
            /*
            var token = new JwtSecurityToken(
                "JWTAuthenticationServer",
                "JWTServicePostmanClient",
                claims);
            */

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));


        }


    }

    public class UserInfo
    {
        public int UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
