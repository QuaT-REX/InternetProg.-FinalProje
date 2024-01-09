using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using sorucevap.Models;
using sorucevap.ViewModel;
using System.Diagnostics;
using System.Security.Claims;
using NETCore.Encrypt.Extensions;
using Microsoft.AspNetCore.Identity;

namespace sorucevap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly Context _c;
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly SignInManager<IdentityAppUser> _signInManager;
        private readonly RoleManager<IdentityAppRole> _role;
        public HomeController(ILogger<HomeController> logger, IConfiguration config, Context c, UserManager<IdentityAppUser> userManager, SignInManager<IdentityAppUser> signInManager, RoleManager<IdentityAppRole> role)
        {
            _logger = logger;
            _config = config;
            _c = c;
            _userManager = userManager;
            _signInManager = signInManager;
            _role = role;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Cookie baazlı
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Login(LoginModel model)
        //{
        //    var hashedpass = MD5Hash(model.Sifre);
        //    var user = _c.Users.Where(s => s.KullaniciAdi == model.KullaniciAdi && s.Sifre == hashedpass).SingleOrDefault();

        //    if (user == null)
        //    {
        //        ViewBag.message = "Kullanıcı adı veya parola yanlış";
        //    }

        //    List<Claim> claims = new List<Claim>() {

        //        new Claim(ClaimTypes.Name, user.Adi),
        //        new Claim(ClaimTypes.Surname, user.Soyadi),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Role,user.Rol),
        //        new Claim("KullaniciAdi",user.KullaniciAdi),
        //    };
        //    ClaimsIdentity idetity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    ClaimsPrincipal principal = new ClaimsPrincipal(idetity);

        //    AuthenticationProperties properties = new AuthenticationProperties()
        //    {
        //        AllowRefresh = true,
        //    };

        //    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
        //    HttpContext.Session.SetInt32("UserId", user.Id);
        //    return RedirectToAction("Index");
        //}

        //public string MD5Hash(string pass)
        //{
        //    var salt = _config.GetValue<string>("AppSettings:MD5Salt");
        //    var password = pass + salt;
        //    var hashed = password.MD5();
        //    return hashed;
        //}


        //public IActionResult Logout()
        //{
        //    HttpContext.SignOutAsync();
        //    return RedirectToAction("Login");
        //}

        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Register(RegisterModel model)
        //{
        //    if (_c.Users.Count(s => s.KullaniciAdi == model.KullaniciAdi) > 0)
        //    {
        //        ViewBag.message("Girilen Kullanıcı Adı Kayıtlıdır!");

        //    }
        //    if (_c.Users.Count(s => s.Email == model.Email) > 0)
        //    {
        //        ViewBag.message("Girilen E-Posta Adresi Kayıtlıdır!");

        //    }



        //    var hashedpass = MD5Hash(model.Sifre);
        //    var user = new AppUser();
        //    user.Adi = model.Adi;
        //    user.Soyadi = model.Soyadi;
        //    user.KullaniciAdi = model.KullaniciAdi;
        //    user.Sifre = hashedpass;
        //    user.Email = model.Email;
        //    user.Rol = "Kullanıcı";
        //    _c.Users.Add(user);
        //    _c.SaveChanges();



        //    return RedirectToAction("Login");
        //} 
        #endregion


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                if (user == null)
                {
                    ViewBag.message = "Böyle bir kullanıcı bulunamadı";
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Sifre, false, false);

                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Kullanici"))
                        {
                            HttpContext.Session.SetInt32("UserId", user.Id);
                            HttpContext.Session.SetString("name", user.Name);
                            HttpContext.Session.SetString("surname", user.Surname);

                            return Redirect("/Soru/Index/");
                        }
                        else
                        {
                            HttpContext.Session.SetString("name", user.Name);
                            HttpContext.Session.SetString("surname", user.Surname);
                            HttpContext.Session.SetInt32("UserId", user.Id);
                            return Redirect("/Soru/GelenSorular");
                        }

                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityAppUser
                {
                    Name = model.Adi,
                    Surname = model.Soyadi,
                    UserName = model.KullaniciAdi,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Sifre);
                if (result.Succeeded)
                {

                    var userRole = await _userManager.FindByEmailAsync(model.Email);

                    //Yeni rol ekleme
                    //var roleExist = await _role.RoleExistsAsync("Kullanici");
                    //if (!roleExist)
                    //{
                    //    var role = new IdentityAppRole { Name = "Kullanici" };
                    //    await _role.CreateAsync(role);
                    //}

                    //Kullanıcıya rol atama
                    await _userManager.AddToRoleAsync(userRole, "Kullanici");

                    return RedirectToAction("Login");
                }
            }
            return View();
        }

    }
}


