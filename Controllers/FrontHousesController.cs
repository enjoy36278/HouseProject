using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class FrontHousesController : Controller
    {
        private readonly dbHouseContext _context;

        public FrontHousesController(dbHouseContext context)
        {
            _context = context;
        }

        // GET: FrontHouses
        // 前台-房屋資料
        public async Task<IActionResult> Index(string? sort, int cityID = 1, int typeID = 0)
        {
            var dbHouseContext =  _context.House.Where(h => h.CityID == cityID);

            //typeID有傳值才做房屋類型篩選
            if (typeID != 0)
            {
                dbHouseContext = dbHouseContext.Where(h => h.TypeID == typeID);
            }

            //排序
            if (sort != null)
            {               
                dbHouseContext = sort switch
                {
                    "rent_asc" => dbHouseContext.OrderBy(h => h.Rent),
                    "rent_desc" => dbHouseContext.OrderByDescending(h => h.Rent),
                    "square_asc" => dbHouseContext.OrderBy(h => h.Square),
                    "square_desc" => dbHouseContext.OrderByDescending(h => h.Square),
                    _ => dbHouseContext // 預設情況，不做排序
                };

            }


            dbHouseContext = dbHouseContext.Include(h => h.City).Include(h => h.Member).Include(h => h.Type);

            VMHouseCityType houseCityType = new VMHouseCityType()
            {
                Houses = await dbHouseContext.ToListAsync(),
                Cities = await _context.City.ToListAsync(),
                HouseTypes = await _context.HouseType.ToListAsync()
            };

            ViewData["cityID"] = cityID;
            ViewData["typeID"] = typeID;

            var city = await _context.City.FindAsync(cityID);
            ViewData["cityName"] = city.CityName;

            return View(houseCityType);
        }

        // GET: FrontHouses/Details/5
        // 前台-房屋詳細資料
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

        // GET: FrontHouses/Create
        //public IActionResult Create(int? cityid)
        //{
        //    ViewData["CityID"] = new SelectList(_context.City, "CityID", "CityName");
        //    ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName");
        //    ViewData["TypeID"] = new SelectList(_context.HouseType, "TypeID", "TypeName");
        //    ViewData["passCityID"] = cityid;
        //    return View();
        //}

        // POST: FrontHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("HouseID,HouseName,Square,Rent,CityID,District,Address,Note,TypeID,MemberID")] House house)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(house);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index", new { cityID = house.CityID});
        //    }
        //    ViewData["CityID"] = new SelectList(_context.City, "CityID", "CityName", house.CityID);
        //    ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName", house.MemberID);
        //    ViewData["TypeID"] = new SelectList(_context.HouseType, "TypeID", "TypeName", house.TypeID);
        //    return View(house);
        //}


        //前台-刊登房屋
        public IActionResult Create2(int? cityid)
        {
            ViewData["CityID"] = new SelectList(_context.City, "CityID", "CityName");
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName");
            ViewData["TypeID"] = new SelectList(_context.HouseType, "TypeID", "TypeName");
            ViewData["passCityID"] = cityid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(House house, IFormFile? file)
        {
            // 有上傳檔案
            if (file != null && file.Length > 0)
            {
                //限制檔案格式
                if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
                {
                    ViewData["Message"] = "請上傳jpg或png格式的檔案!!";
                    ViewData["CityID"] = new SelectList(_context.City, "CityID", "CityName", house.CityID);
                    ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName", house.MemberID);
                    ViewData["TypeID"] = new SelectList(_context.HouseType, "TypeID", "TypeName", house.TypeID);

                    return View(house);
                }

                // 定義上傳檔案儲存的資料夾路徑              
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");

                // 生成唯一的檔案名稱，使用 GUID 來避免檔名衝突，並保留原始檔案的副檔名
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // 組合上傳資料夾路徑與唯一檔案名稱，形成完整的檔案路徑
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 開啟檔案流以寫入目標檔案，並將上傳檔案的內容複製進去
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    // 非同步地將上傳檔案複製到新建的檔案流中
                    await file.CopyToAsync(fileStream);
                }

                house.PhotoName = uniqueFileName;             

            }


            //寫入資料庫
            if (ModelState.IsValid)
            {
                _context.Add(house);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { cityID = house.CityID });
            }

            ViewData["CityID"] = new SelectList(_context.City, "CityID", "CityName", house.CityID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName", house.MemberID);
            ViewData["TypeID"] = new SelectList(_context.HouseType, "TypeID", "TypeName", house.TypeID);
            return View(house);
        }


        private bool HouseExists(int id)
        {
            return _context.House.Any(e => e.HouseID == id);
        }
    }
}
