using AspNetCoreHero.ToastNotification.Abstractions;
using BlogPost.web.Models;
using BlogPost.web.Utilities;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; //this is for creating or deleting user
        private readonly SignInManager<ApplicationUser> _signInManager; //this  is for signing in and signing out purpose
        private readonly INotyfService _notification; // this is for toaster notification alert
        public UserController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, INotyfService _notification)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;   
            this._notification = _notification;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users= await _userManager.Users.ToListAsync();
            var vm = users.Select(x => new UserVm()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
            }).ToList();

            foreach(var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                user.Roles = role.FirstOrDefault();
            }
              return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notification.Error("User doesn't exist");
                return View();
            }
            var vm = new ResetPasswordVm()
            {
                Id = existingUser.Id,
                Username = existingUser.UserName,
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm vm)
        {
            if (!ModelState.IsValid) { return View(vm); }

            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if(existingUser == null)
            {
                _notification.Error("User not found");
                return View(vm);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.newPassword);
            if (result.Succeeded)
            {
                _notification.Success("Password Reset Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegsiterVm vm)
        {
            if (!ModelState.IsValid) {  return View(vm); }

            var checkUserByEmail= await _userManager.FindByEmailAsync(vm.Email);
            if (checkUserByEmail != null)
            {
                _notification.Error("Email already exists");
                return View(vm);
            }

            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);
            if (checkUserByUsername != null)
            {
                _notification.Error("Username already exists");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                UserName= vm.UserName,
                FirstName=vm.FirstName,
                LastName=vm.LastName,
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if (result.Succeeded)
            {
                if (vm.isAdmin)
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
                }

                _notification.Success("User registered successfully");
               return RedirectToAction("Index", "User", new {area="Admin"});
            }
                return View(vm);
        }

        [HttpGet("Login")]
        public  IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }

            var checkuser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.Username);
            if (checkuser == null) {
                _notification.Error("Username does not found");
                return View(vm);    
            }

            var checkPassword = await _userManager.CheckPasswordAsync(checkuser, vm.Password);
            if (checkuser == null)
            {
                _notification.Error("Password does not match");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);
            _notification.Success("Login successfully");
            return RedirectToAction("Index","Post", new{area="Admin"});
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("Logged out successfullly");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet("AccessDenied")]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
