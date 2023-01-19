using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.Models.Domain;
using NewsApp.Models.DTO;
using NewsApp.Repositories.Abstract;

namespace NewsApp.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DatabaseContext _context;


        public UserAuthenticationController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager, DatabaseContext context)
        {
            _service = service;
            _userManager = userManager;
            _context = context;
       

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            var result = await _service.LoginAsync(model);
            if (result.StatusCode==1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            model.Role = "user";
            var result = await _service.RegistrationAsync(model);
            TempData["msg"]=result.Message;
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

         //this method used for only creating admin
        //public async Task<IActionResult> reg()
        //{
        //    var model = new RegistrationModel
        //    {
        //        UserName = "admin",
        //        Name = "admin",
        //        Email = "admin@admin.com",
        //        Password = "Admin@12345#"
        //    };
        //    model.Role = "admin";
        //    var result = await _service.RegistrationAsync(model);
        //    return Ok(result);

        //}

        [Authorize(Roles = "admin")]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

       


    }
}
