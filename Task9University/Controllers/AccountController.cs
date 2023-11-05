using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;
using Task9.University.Infrastructure.Presentations.IdentityViewModels;

namespace Task9University.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Idenx()
    {
        return View();
    }

    public async Task<IActionResult> Register(string? returnUrl = null)
    {
        RegisterViewModel registerVM = new RegisterViewModel();
        registerVM.ReturnUrl = returnUrl;
        return View(registerVM);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerVM, string? returnUrl = null)
    {
        registerVM.ReturnUrl = returnUrl;
        returnUrl = returnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            var user = new User { Email = registerVM.Email, UserName = registerVM.Email };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            ModelState.AddModelError("Password", "User could not be created. Password not unique enough");
        }
        return View(registerVM);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        LoginViewModel loginViewModel = new LoginViewModel();
        loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
        return View(loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginVM, string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password!");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, loginVM.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid loging attempt. ");
                return View(loginVM);
            }
        }
        return View(loginVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
