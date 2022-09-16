﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace SecurityTests.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class ApiAuthController : ControllerBase
    {
        private SignInManager<IdentityUser> SignInManager;
        private UserManager<IdentityUser> UserManager;
        private IConfiguration Configuration;
        public ApiAuthController(SignInManager<IdentityUser> signMgr,
UserManager<IdentityUser> usrMgr,
IConfiguration config)
        {
            SignInManager = signMgr;
            UserManager = usrMgr;
            Configuration = config;
        }

        [HttpPost("signin")]
        public async Task<object> ApiSignIn(
        [FromBody] SignInCredentials creds)
        {
            IdentityUser user = await UserManager.FindByEmailAsync(creds.Email);
            Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.CheckPasswordSignInAsync(user, creds.Password, true);
            
            if (result.Succeeded)
            {
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {

                    Subject = (await SignInManager.CreateUserPrincipalAsync(user)).Identities.First(),
                    Expires = DateTime.Now.AddMinutes(int.Parse(Configuration["BearerTokens:ExpiryMins"])),
                    SigningCredentials = new SigningCredentials( 
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"]))
                            , SecurityAlgorithms.HmacSha256Signature
                    )
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken secToken = new JwtSecurityTokenHandler().CreateToken(descriptor);
                return new { success = true, token = handler.WriteToken(secToken) };
            }
            return new { success = false };
        }

            [HttpPost("signout")]
        public async Task<object> ApiSignOut()
        {
            await SignInManager.SignOutAsync();
            return new { success = true };
        }

        
    }
    public class SignInCredentials
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}