using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Forum.Repositorys;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
namespace Forum.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserRepository _context;

        public UserController(UserRepository context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _context.GetAll();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: User/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            try
            {
                await _context.Update(user);
                return View(user);
            }
            
            catch(ArgumentNullException ex)
            {
                return View(user);
            }
            catch (Exception ex)
            {
                return View();
            }



        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _context.DeleteById(id);
                return View();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound();
            }
            
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Guid getIdFromToken()
        {
            var tokenString = HttpContext.Session.GetString("Token");
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenString);
            var claim = jwt.Claims.Where(claim => claim.Type.Contains("sid"));
            var id = Guid.Parse(claim.FirstOrDefault().Value);
            return id;
        }

        [Authorize]
        [HttpGet]
        public async Task<User> CurrentUser()
        {
            var id = getIdFromToken();

            return await _context.GetById(id) ?? throw new NullReferenceException();
        }


    }
}
