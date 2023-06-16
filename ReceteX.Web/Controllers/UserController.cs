using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Security.Claims;

namespace ReceteX.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        //  [Route("User/{customerId?}")]
        public IActionResult Index()
        {

            return View();
        }

        //admin olarak girildiğinde tüm user'ları listeleyeceğimiz action.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll([FromQuery(Name = "customerId")] string? customerId)
        {
            if (customerId==null)
                //DataTables oluşabilmek için verilerin hepsinin DATA isimli bir obje içinde gelme kuralı koyuyor. O yuzden aşağıdaki gibi yeni bir anonim nesne oluşturup içerisine data diye bir field açıyoruz ve bütün datayı onun içine koyuyoruz. Bunu sırf DataTables için yapıyoruz.
                return Json(new { data = unitOfWork.Users.GetAll().Include(u => u.Customer) });
            else
                return Json(new { data = unitOfWork.Users.GetAll(u => u.CustomerId==Guid.Parse(customerId)).Include(u => u.Customer) });

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetById(string id)
        {
            AppUser usr = unitOfWork.Users.GetFirstOrDefault(u => u.Id == Guid.Parse(id));
            return Json(usr);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(AppUser appUser)
        {
            unitOfWork.Users.Add(appUser);
            unitOfWork.Save();
            return Ok();

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(AppUser appUser)
        {
            AppUser asil = unitOfWork.Users.GetFirstOrDefault(u => u.Id==appUser.Id);

            appUser.DateCreated = asil.DateCreated;
            appUser.isRememberMe = asil.isRememberMe;
            appUser.isActive = asil.isActive;


            unitOfWork.Users.Update(appUser);
            unitOfWork.Save();
            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AppUser user)
        {
            if (user != null)
            {
                AppUser usr = unitOfWork.Users.GetFirstOrDefault(u => u.Email==user.Email&& u.Password==user.Password && u.isActive && !u.isDeleted);
                if (usr != null)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, usr.Name + " "+ usr.Surname));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()));
                    if (usr.isAdmin)
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    else
                        claims.Add(new Claim(ClaimTypes.Role, "User"));




                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = usr.isRememberMe });

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ChangeStatus(Guid id)
        {
            AppUser asil = unitOfWork.Users.GetById(id);
            if (asil.isActive)
            {
                asil.isActive = false;
                unitOfWork.Users.Update(asil); unitOfWork.Save();
                return Ok();
            }
            else
            {
                asil.isActive = true; unitOfWork.Users.Update(asil);
                unitOfWork.Save(); return Ok();
            }
        }
    }
}