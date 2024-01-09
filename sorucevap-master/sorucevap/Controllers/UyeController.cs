using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sorucevap.Models;
using sorucevap.ViewModel;

namespace sorucevap.Controllers
{
    public class UyeController : Controller
    {
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly RoleManager<IdentityAppRole> _roleManager;

        public UyeController(UserManager<IdentityAppUser> userManager, RoleManager<IdentityAppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.Select(x=>new UserList
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Surname = x.Surname,
                UserName = x.Surname
            }).ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Yetkilendirme(int id)
        {
            var user = _userManager.Users.FirstOrDefault
                 (x => x.Id == id);

            var roles = _roleManager.Roles.ToList();

            TempData["Userid"] = user.Id;

            var userRoles = await _userManager.GetRolesAsync(user);

            List<YetkilendirmeModel> yetkiModel = new List<YetkilendirmeModel>();

            foreach (var item in roles)
            {
                YetkilendirmeModel m = new YetkilendirmeModel();
                m.RoleID = item.Id;
                m.Name = item.Name;
                m.Exists = userRoles.Contains(item.Name);
                yetkiModel.Add(m);
            }

            return View(yetkiModel);
        }

        [HttpPost]
        public async Task<IActionResult> Yetkilendirme(List<YetkilendirmeModel> model)
        {
            string username = HttpContext.Session.GetString("UserName");
            int userid = (int)TempData["Userid"];
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userid);
            foreach (var item in model)
            {
                if (item.Exists)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);                   
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);

                }

            }
            return Redirect("/Uye/Index/");
        }
    }
}
