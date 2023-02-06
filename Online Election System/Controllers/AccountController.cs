using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Election_System.Models.ViewModels;
using System.Security.Claims;

namespace Online_Election_System.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		[TempData]
		public string? Message { get; set; }
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{

			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpGet]
		public IActionResult Login(string? returnUrl = null)
		{
			ViewBag.Title = "Signin";

			return View(new LoginViewModel
			{
				ReturnUrl = returnUrl
			});
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			ViewBag.Title = "Signin";
			try
			{
				if (ModelState.IsValid)
				{
					var user = await _userManager.FindByNameAsync(model.UserName);
					
					if (user != null)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
						if (result.Succeeded)
							return RedirectToAction("Index", "Home");
					}
				}
				return View(model);
			}
			catch (Exception x)
			{
				ModelState.AddModelError("", x.Message);
				return View(model);
			}
		}

		[HttpGet]
		public IActionResult Register()
		{
			ViewBag.Title = "Sign up";
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{

			ViewBag.Title = "Sign up";
			try
			{
				if (ModelState.IsValid)
				{
					IdentityUser user = new IdentityUser() { Email = model.Email, UserName = model.UserName };
					var results = await _userManager.CreateAsync(user, model.Password);
					if (results.Succeeded)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
						if (result.Succeeded)
						{
							ViewData["ID"] = user.Id;
							return RedirectToAction("Index", "Home");
						}
						return View("Login");
					}
				}
				return View(model);
			}
			catch (Exception x)
			{

				ModelState.AddModelError("", x.Message);
				return View(model);
			}
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public IActionResult ExternalProvider(string provider, string? returnurl = null)
		{
			//request a redirect to the external login provider
			var redirecturl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnurl });
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirecturl);
			return Challenge(properties, provider);
		}
		[HttpGet]
		public async Task<IActionResult> ExternalLoginCallback(string? returnurl = null, string? remoteError = null)
		{
			if (remoteError != null)
			{
				ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
				return View(nameof(Login));
			}
			var info = await _signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				return RedirectToAction(nameof(Login));
			}

			//Sign in the user with this external login provider, if the user already has a login.
			var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
			if (result.Succeeded)
			{
				//update any authentication tokens
				await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				//If the user does not have account, then we will ask the user to create an account.
				ViewData["ReturnUrl"] = returnurl;
				ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
				var email = info.Principal.FindFirstValue(ClaimTypes.Email);

				return View("ExternalLoginConfirmation", new ExternalLoginViewModel { Email = email });
			}
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public IActionResult ExternalLogin(string provider, string? returnurl = null)
		{
			//request a redirect to the external login provider
			var redirecturl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnurl });
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirecturl);
			return Challenge(properties, provider);
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string? returnurl = null)
		{
			returnurl = returnurl ?? Url.Content("~/");

			if (ModelState.IsValid)
			{
				//get the info about the user from external login provider
				var info = await _signInManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("Error");
				}
				var user = new IdentityUser() { UserName = model.UserName, Email = model.Email };
				var result = await _userManager.CreateAsync(user);
				if (result.Succeeded)
				{
					// await _userManager.AddToRoleAsync(user, "TestUser");
					result = await _userManager.AddLoginAsync(user, info);
					if (result.Succeeded)
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
						return RedirectToAction("Index", "Home");
					}
				}
				ModelState.AddModelError("Email", "Error occured");
			}
			ViewData["ReturnUrl"] = returnurl;
			return View(model);
		}
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			ViewBag.Title = "Loging out";
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}