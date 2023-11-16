using Forum.Models;
using Forum.Repositorys;
using Forum.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Forum.Controllers
{
    public class SecurityController : Controller
    {
        private readonly UserRepository _context;
        private readonly IJwtService _jwtService;

        public SecurityController(UserRepository userRepository, IJwtService jwtService)
        {
            _context = userRepository;
            _jwtService = jwtService;
        }

        // GET: SecurityController
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("test");
            }
            var loggedInUser = await _context.ValidUser(model);

            if (loggedInUser == null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, loggedInUser.Pseudonyme),
        new Claim(ClaimTypes.Email, loggedInUser.Email),
        new Claim(ClaimTypes.Sid, loggedInUser.Id.ToString())
    };
            if (loggedInUser.Admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            var tokenString = _jwtService.GenerateToken(loggedInUser, claims);
            HttpContext.Session.SetString("Token", tokenString);
            return Redirect("http://localhost:5109/User");
        }
    }
}
