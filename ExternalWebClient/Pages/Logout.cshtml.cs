using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalWebClient.Pages
{
    [ValidateAntiForgeryToken]
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
