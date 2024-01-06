using AuthenticationAndAuthorizationMVC.Models.DTO_s;
using AuthenticationAndAuthorizationMVC.Models.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AuthenticationAndAuthorizationMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        #region Registration
        [AllowAnonymous] //Herkes bu action'a erişebilir!
        public IActionResult Register() => View();

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser { UserName = model.UserName, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    TempData["Succces"] = "Kayıt başarılı. Giriş yapabilirsiniz!!";
                    return RedirectToAction("LogIn");
                }
                else
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        #endregion

        #region Login
        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl)
        {
            var model = new LogInDTO { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInDTO model)
        {
            if (ModelState.IsValid) 
            {
                AppUser user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null) 
                {
                    SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Hoşgeldiniz " + user.UserName;
                        return Redirect(model.ReturnUrl ?? "/");
                    }
                    else
                    {
                        TempData["Warning"] = "Kullanıcı adı veya şifre yanlış!!";
                        ModelState.AddModelError("", "Hatalı giriş!!");
                    }
                }
                return NotFound();
            }
            return View(model);
        }
        #endregion

        #region Edit User

        public async Task<IActionResult> Edit()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateDTO model = new UserUpdateDTO(user);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.UserName = model.UserName;
                if (model.Password != null)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                }
                user.Email = model.Email;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Kullanıcı bilgiler güncellendi!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Warning"] = "Bilgileriniz güncellenemedi!!";
                }
            }
            return View(model);
        }
        #endregion

        #region LogOut

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Warning"] = "Çıkış Yapıldı!!";
            return RedirectToAction("LogIn");
        }

        #endregion
    }
}
