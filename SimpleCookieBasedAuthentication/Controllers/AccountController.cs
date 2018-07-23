using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleCookieBasedAuthentication.Models.Account;
using SimpleCookieBasedAuthentication.Models.Common;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCookieBasedAuthentication.Controllers
{
    public class AccountController : Controller
    {
        [BindProperty]
        public LoginData loginData { get; set; }

        private readonly AdminConfig adminConfig;

        public AccountController(IOptions<AdminConfig> options)
        {
            adminConfig = options.Value;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            if (ModelState.IsValid)
            {
                var isValid = (loginData.Username == adminConfig.UserName && loginData.Password == adminConfig.Password); // TODO Validate the username and the password with your own logic
                if (!isValid)
                {
                    ModelState.AddModelError("", "username or password is invalid");
                    return View("Login");
                }
                // Create the identity from the user info
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginData.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, loginData.Username));
                // Authenticate using the identity
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = loginData.RememberMe });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "username or password is blank");
                return View("Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}