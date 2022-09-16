using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SecurityTests.Controllers
{
    [ApiController]
    [Route("/api/dashboard")]
    public class DashboardController : ControllerBase
    {
        public DashboardController(UserManager<IdentityUser> userMgr) => UserManager = userMgr;
        public UserManager<IdentityUser> UserManager { get; set; }


        private readonly string[] emails = {
"alice@example.com", "bob@example.com", "charlie@example.com"
};
        [HttpGet("create")]
        public async Task<object> Create()
        {
            foreach (string email in emails)
            {
                IdentityUser userObject = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult result = await UserManager.CreateAsync(userObject);
                if (!result.Succeeded)
                    return new { success = false };
                
                result = await UserManager.AddPasswordAsync(userObject, "Password01_");
                if (!result.Succeeded)
                    return new { success = false };
            }

            return new { success = true };
        }
    }
}