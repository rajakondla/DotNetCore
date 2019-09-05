using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPracticeWebSite.IdentityUserModel;
using MyPracticeWebSite.ViewModels;

namespace MyPracticeWebSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<MyPracticeUserIdentity> _userManager;
        private readonly SignInManager<MyPracticeUserIdentity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<MyPracticeUserIdentity> userManager,SignInManager<MyPracticeUserIdentity> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
          
        }

        public async Task<IActionResult> Register()
        {
            if (!await _roleManager.RoleExistsAsync("Organiser"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "Organiser" });
            if (!await _roleManager.RoleExistsAsync("Speaker"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "Speaker" });
            return View();
        }

        public IActionResult Login(string returnUrl="")
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
                return View("Error");

            var user = new MyPracticeUserIdentity
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                DOB=registerModel.BirthDate
            };
            var result = await _userManager.CreateAsync(user, password:registerModel.Password);

           await _userManager.AddToRoleAsync(user, registerModel.Role);
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("technology",registerModel.Technology)); 

            if (result.Succeeded)
                return RedirectToAction("Index", "Conference");

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel,string returnUrl="")
        {
            if (!ModelState.IsValid)
                return View("Error");
            ViewData["returnUrl"] = returnUrl;
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, lockoutOnFailure:false);

            if(result.Succeeded)
            {
                if(Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index","Conference");
                }
            }

            if(result.RequiresTwoFactor)
            {

            }

            if(result.IsLockedOut)
            {
                return View("Lockout");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(loginModel);
        }

        public async Task<IActionResult> Logoff()
        {
            await _signInManager.SignOutAsync();
            return View("LoggedOut");
        }
    }
}