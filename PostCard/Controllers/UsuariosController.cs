using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostCard.Models;
using PostCard.Models.ViewModel;

namespace PostCard.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AplicationContext _context;

        public UsuariosController(AplicationContext context)
        {
            _context = context;
        }


        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (await EmailUnique(usuario))
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewData["error"] = "The email is already registered";
                }

            }
            return View(usuario);
        }
        public IActionResult Login()
        {
            var cookie = Request.Cookies["user"];
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated && cookie != null)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuarioLogincs log)
        {
            if (ModelState.IsValid)
            {
                var userEmail = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == log.Email);
                if (userEmail != null)
                {
                    if (userEmail.Password == log.Password)
                    {
                        List<Claim> c = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, userEmail.email)
                        };
                        ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties p = new();

                        p.AllowRefresh = true;
                        p.IsPersistent = true;
                        p.ExpiresUtc = DateTimeOffset.MaxValue;
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);

                        

                        Response.Cookies.Append("user", userEmail.email, new CookieOptions
                        {
                            Expires = DateTimeOffset.MaxValue
                        });
                        return RedirectToAction("Index", "Home");


                    }
                    else
                    {
                        ViewData["error"] = "The password is incorrect";
                    }
                }
                else
                {
                    ViewData["error"] = "The email is not registered";
                }
            }
            return View(log);
        }

        //validando si el correo es no esta repetido
        public async Task<bool> EmailUnique(Usuario user)
        {
            return await Task.FromResult(!await _context.Usuarios.AnyAsync(e => e.email == user.email));
        }
    }
}
