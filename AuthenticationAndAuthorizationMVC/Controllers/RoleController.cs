using AuthenticationAndAuthorizationMVC.Models.DTO_s;
using AuthenticationAndAuthorizationMVC.Models.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAndAuthorizationMVC.Controllers
{
    //[Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleDTO model)
        {
            if (ModelState.IsValid) 
            {
                //Uzun Hali
                //var role = new IdentityRole(model.Name);
                //IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(role.Name));

                //Kısa Hali
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role başarıyla kaydedildi!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        TempData["Warning"] = error.Description; 
                    }
                }
            }
            TempData["Warning"] = "Aşağıdaki kurallara uyunuz!!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignedUser(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            List<AppUser> hasRole = new List<AppUser>();
            List<AppUser> hasNotRole = new List<AppUser>();

            foreach (var user in _userManager.Users)
            {
                //Uzun Hali
                //if (await _userManager.IsInRoleAsync(user, role.Name))
                //{
                //    hasRole.Add(user);
                //}
                //else
                //{
                //    hasNotRole.Add(user);
                //}

                //Kısa Hali
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? hasRole : hasNotRole;
                list.Add(user);
            }

            AssignedRoleDTO model = new AssignedRoleDTO
            {
                Role = role,
                RoleName = role.Name,
                HasRole = hasRole,
                HasNotRole = hasNotRole
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignedUser(AssignedRoleDTO model)
        {
            IdentityResult result;

            foreach (var userId in model.AddIds ?? new string[] { })
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            foreach (var userId in model.DeleteIds ?? new string[] { })
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            }

            TempData["Success"] = "Kullanıcılar başarılı bir şekilde atandılar!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                TempData["Success"] = "Rol silindi!!";
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
