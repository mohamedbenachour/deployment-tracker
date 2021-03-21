/*
 * This file is part of Deployment Tracker.
 * 
 * Deployment Tracker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Deployment Tracker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using DeploymentTrackerCore.Services.Identity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Views.Account {
    [AllowAnonymous]
    public class LoginModel : PageModel {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        private IUserStore<ApplicationUser> UserStore { get; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IUserStore<ApplicationUser> userStore) {
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

        public class InputModel {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null) {
            if (!string.IsNullOrEmpty(ErrorMessage)) {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid) {
                var user = await UserStore.FindByNameAsync(Input.UserName, CancellationToken.None);

                if (user != null) {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);

                    if (result.Succeeded) {
                        var identity = user.GetClaimIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        var sessionExpiry = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddHours(1);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity),
                            new AuthenticationProperties {
                                AllowRefresh = true,
                                    ExpiresUtc = sessionExpiry
                            });

                        if (Url.IsLocalUrl(returnUrl)) {
                            return Redirect(returnUrl);
                        } else {
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