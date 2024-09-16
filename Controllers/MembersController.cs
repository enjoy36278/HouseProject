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
    public class MembersController : Controller
    {
        private readonly dbHouseContext _context;

        public MembersController(dbHouseContext context)
        {
            _context = context;
        }

        // GET: Members
        // 後台-會員資料管理
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        //// GET: Members/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = await _context.Member
        //        .FirstOrDefaultAsync(m => m.MemberID == id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(member);
        //}

        // GET: Members/Create
        // 前台-加入會員
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberID,MemberName,Gender,Phone")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "會員資料新增成功";
                return RedirectToAction("Index", "FrontHouses");
                //ViewData["Message"] = "會員資料新增成功";
                //return View();
            }
            return View(member);
        }

        // GET: Members/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = await _context.Member.FindAsync(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(member);
        //}

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("MemberID,MemberName,Gender,Phone")] Member member)
        //{
        //    if (id != member.MemberID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(member);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MemberExists(member.MemberID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(member);
        //}

        // GET: Members/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = await _context.Member
        //        .FirstOrDefaultAsync(m => m.MemberID == id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(member);
        //}

        // GET: Members/Delete/5
        // 刪除會員-會員詳細資料和房屋資料顯示
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VMHouseMember vmHouseMember = new VMHouseMember()
            {
                Houses = await _context.House.Where(h => h.MemberID == id).Include(h => h.City).Include(h => h.Type).ToListAsync(),
                Members = await _context.Member.Where(m => m.MemberID == id).ToListAsync()
            };

            if (vmHouseMember == null)
            {
                return NotFound();
            }

            return View(vmHouseMember);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.MemberID == id);
        }
    }
}
