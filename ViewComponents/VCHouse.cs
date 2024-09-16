using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.ViewComponents
{
    public class VCHouse:ViewComponent
    {
        private readonly dbHouseContext _context;
        public VCHouse(dbHouseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int memberID)
        {
            var house =  _context.House.Where(h => h.MemberID == memberID).Include(h => h.City).Include(h => h.Type);
            return View(await house.ToListAsync());
        }
    }
}
