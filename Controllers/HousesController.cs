using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class HousesController : Controller
    {
        private readonly dbHouseContext _context;

        public HousesController(dbHouseContext context)
        {
            _context = context;
        }

        // GET: Houses
        // 後台-房屋資料管理
        public async Task<IActionResult> Index(int? houseID)
        {
            IQueryable<House> query = _context.House.Include(h => h.City).Include(h => h.Type);

            //houseID有傳值做房屋篩選
            if (houseID != null)
            {
                query = query.Where(h => h.HouseID == houseID);
                ViewData["houseID"] = houseID;
            }
            return View(await query.ToListAsync());
        }

        // GET: Houses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.House
                .Include(h => h.City)
                .Include(h => h.Member)
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.HouseID == id);
            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }



        // GET: Houses/Delete/5
        // 刪除房屋-詳細資料顯示
        // controllerName用來控制刪除成功的頁面切換
        public async Task<IActionResult> Delete(int? id,string? controllerName)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.House
                .Include(h => h.City)
                .Include(h => h.Member)
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.HouseID == id);
            if (house == null)
            {
                return NotFound();
            }

            ViewData["ControllerName"] = controllerName;
            
            return View(house);
        }

        // POST: Houses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string? controllerName)
        {
            var house = await _context.House.FindAsync(id);
            if (house != null)
            {
                // 刪除資料庫記錄
                _context.House.Remove(house);
                await _context.SaveChangesAsync();

                // 刪除圖片檔案
                if(house.PhotoName != null)
                {
                    var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads", house.PhotoName);
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }
               
            }

            //回到刪除會員資料頁面
            if (controllerName == "Members")
            {
                return RedirectToAction("Delete", "Members", new {id=house.MemberID});
            }

            //回到房屋資料管理頁面
            return RedirectToAction(nameof(Index));
        }

        private bool HouseExists(int id)
        {
            return _context.House.Any(e => e.HouseID == id);
        }
    }
}
