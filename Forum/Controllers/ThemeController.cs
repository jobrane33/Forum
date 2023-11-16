using Forum.Models;
using Forum.Repositorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Forum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ThemeController : Controller
    {
        private readonly ThemeRepository _context;

        public ThemeController(ThemeRepository context)
        {
            _context = context;
        }


        // GET: ThemeController
        public async Task<IActionResult> Index()
        {
            var themes = await _context.GetAll();
            return View(themes);
        }

        // GET: ThemeController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Theme theme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(theme);
        }

        

        // GET: ThemeController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                if (_context is null)
                {
                    throw new ArgumentNullException(nameof(_context));
                }
                var theme = await _context.GetById(id);

                return View(theme);
            }
            catch (Exception)
            {
               return Redirect("http://localhost:5109/Home/Error");
            }
        }

        // POST: ThemeController/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Theme theme)
        {
            try
            {
                var newTheme = await _context.Update(theme);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return Redirect("http://localhost:5109/Home/Error");
            }

            
        }

        // GET: User/Delete/5
        public IActionResult Delete()
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
