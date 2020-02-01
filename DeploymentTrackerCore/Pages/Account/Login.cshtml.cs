using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using DeploymentTrackerCore.Services.Identity;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace DeploymentTrackerCore.Views.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        private IUserStore<ApplicationUser> UserStore { get; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IUserStore<ApplicationUser> userStore)
        {
            _signInManager = signInManager;
            _logger = logger;
            UserStore = userStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                    var user = await UserStore.FindByNameAsync(Input.UserName, CancellationToken.None);

                    if (user != null ) {
                        var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);

                        if (result.Succeeded) {
                            var identity = user.GetClaimIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity),
                            new AuthenticationProperties {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                                IsPersistent = Input.RememberMe
                            });

                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                    }

                    _logger.LogInformation("Did not work");

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
