using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI.Models;
using Microsoft.AspNetCore.Http;

namespace astoriaTrainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("Application/json")]
    public class UserInfoController : Controller
    {
        private readonly astoriaTraining80Context _context;

        public UserInfoController(astoriaTraining80Context context)
        {
            _context = context;
        }

        // GET: UserInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserInfo.ToListAsync());
        }

        // GET: UserInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This method is used saving new user in UserInfo table of SQL Server database.
        /// </summary>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        // POST: UserInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        //{
        //    try
        //    {
        //        if (
        //           string.IsNullOrEmpty(userInfo.FirstName) ||
        //           string.IsNullOrEmpty(userInfo.LastName) ||
        //           string.IsNullOrEmpty(userInfo.UserName) ||
        //          string.IsNullOrEmpty(userInfo.Email) ||
        //          string.IsNullOrEmpty(userInfo.Passwords))
        //        {
        //            return BadRequest();
        //        }
        //        if (_context.UserInfo.Any(uInfo => uInfo.ID == userInfo.ID))
        //        {
        //            return StatusCode(StatusCodes.Status409Conflict);
        //        }
        //        _context.UserInfo.Add(userInfo);
        //        await _context.SaveChangesAsync();
        //        return Ok(userInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        // GET: UserInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,UserName,Email,Passwords,CreationDate")] UserInfo userInfo)
        //{
        //    if (id != userInfo.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(userInfo);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserInfoExists(userInfo.ID))
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
        //    return View(userInfo);
        //}

        // GET: UserInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfo/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var userInfo = await _context.UserInfo.FindAsync(id);
        //    _context.UserInfo.Remove(userInfo);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserInfoExists(int id)
        //{
        //    return _context.UserInfo.Any(e => e.ID == id);
        //}
    }
}
