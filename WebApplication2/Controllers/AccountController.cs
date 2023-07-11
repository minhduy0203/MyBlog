using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            User userCheck = new User();
            using (MyBlogContext ctx = new MyBlogContext())
            {
                userCheck = ctx.Users
                    .Include((u) => u.Role)
                    .FirstOrDefault((u) => u.Email.Equals(user.Email) && u.Password.Equals(user.Password));
            }
            if (userCheck != null)
            {

                String email = userCheck.Email;
                String role = userCheck.Role.Name;
                var claims = new List<Claim>()
              {
                  new Claim(ClaimTypes.Name, email),
                  new Claim(ClaimTypes.Role,role)
              };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = true
                });
                return LocalRedirect("/");

            } else
            {
                ViewBag.Message = "Usernamr or password incorrect";
            }
            return View();
        }


        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/");
        }
    }
}
