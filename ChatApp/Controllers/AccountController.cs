using ChatApp.Models;
using ChatApp.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    [Route("Login")]
    public IActionResult Loginpage()
    {
        return View();
    }
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Loginpage(loginViewModel loginViewModel)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginViewModel.Email) ?? null;
        //TODO: Login User
        if (user == null)
        {
            return View(loginViewModel);
        }
        else
        {
            if (loginViewModel.Password == user.Password)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,loginViewModel.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(principal, properties);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(loginViewModel);
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(loginViewModel loginViewModel)
    {
        var userexist = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginViewModel.Email) ?? null;
        if (String.IsNullOrWhiteSpace(loginViewModel.Email) && String.IsNullOrWhiteSpace(loginViewModel.Password))
        {
            return View(loginViewModel);
        }
        else
        {
            if (userexist == null)
            {
                var user = new User()
                {
                    Password = loginViewModel.Password,
                    Email = loginViewModel.Email,
                    UserStatus = "Offline"
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(loginViewModel);
            }
        }
    }

    #region Logout
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await _context.Users.SingleAsync(u => u.Email == User.Identity.Name);
        user.UserStatus = "Offline";
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        await HttpContext.SignOutAsync();
        return Redirect("/Login");
    }
    #endregion

}