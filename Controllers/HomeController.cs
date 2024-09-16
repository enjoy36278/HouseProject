using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbHouseContext _context;

        public HomeController(ILogger<HomeController> logger, dbHouseContext context)
        {
            _logger = logger;
            _context = context;
        }      

        public async Task<IActionResult> Index()
        {
            var result = _context.House.Where(h => h.PhotoName != null).Include(h => h.City).OrderByDescending(h=>h.HouseID).Take(3);
            return View(await result.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
