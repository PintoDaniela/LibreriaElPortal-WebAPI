using ElPortalApp.Interfaces;
using ElPortalApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElPortalApp.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _servicioAccount;

        public AccountController(IAccountService servicioAccount)
        {
            _servicioAccount = servicioAccount;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _servicioAccount.Login(loginViewModel);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(loginViewModel);           
        }
    }
}
