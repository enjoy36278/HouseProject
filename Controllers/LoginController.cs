using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Models;

namespace Project.Controllers
{
    public class LoginController : Controller
    {
        private readonly dbHouseContext _context;

        public LoginController(dbHouseContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login) {

            if (login == null)
            {
                return View();
            }

            var result = await _context.Login.Where(m => m.Account == login.Account && m.Password == login.Password).FirstOrDefaultAsync();

            if (result == null)
            {
                ViewData["Error"] = "帳號或密碼錯誤!!";
                return View();
            }

            //登入成功
            HttpContext.Session.SetString("Manager", JsonConvert.SerializeObject(login));

            return RedirectToAction("Index", "Houses");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Manager");
            return RedirectToAction("Index", "Home");
        }
    }
}
