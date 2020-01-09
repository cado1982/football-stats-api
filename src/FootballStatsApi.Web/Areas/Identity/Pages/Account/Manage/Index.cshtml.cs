using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootballStatsApi.Domain.Entities.Identity;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            IEmailSender emailSender,
            UserManager<User> userManager,
            ILogger<IndexModel> logger,
            SignInManager<User> signInManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [BindProperty]
        public EmailChangeInputModel EmailChangeInput { get; set; }

        public EmailChangeViewModel EmailChangeView { get; set; }

        [TempData]
        public string EmailChangeStatusMessage { get; set; }

        [TempData]
        public string TwoFactorStatusMessage { get; set; }

        [TempData]
        public string ChangePasswordStatusMessage { get; set; }

        [BindProperty]
        public TwoFactorInputModel TwoFactorInput { get; set; }

        public TwoFactorViewModel TwoFactorView { get; set; }

        public ApiKeyViewModel ApiKeyView { get; set; }

        [BindProperty]
        public ChangePasswordInputModel ChangePasswordInput { get; set; }

        public class EmailChangeInputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        public class ChangePasswordInputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class TwoFactorViewModel
        {
            public bool HasAuthenticator { get; set; }
            public int RecoveryCodesLeft { get; set; }
            public bool IsMachineRemembered { get; set; }
        }

        public class TwoFactorInputModel
        {
            public bool Is2faEnabled { get; set; }
        }

        public class ApiKeyViewModel
        {
            public Guid ApiKey { get; set; }
        }

        public class EmailChangeViewModel
        {
            public bool IsEmailConfirmed { get; set; }
            public string Email { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var email = await _userManager.GetEmailAsync(user);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            var hasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            var is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var isMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            var recoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            EmailChangeInput = new EmailChangeInputModel
            {
                NewEmail = email,
            };

            EmailChangeView = new EmailChangeViewModel
            {
                Email = email,
                IsEmailConfirmed = isEmailConfirmed
            };

            TwoFactorInput = new TwoFactorInputModel
            {
                Is2faEnabled = is2faEnabled
            };

            TwoFactorView = new TwoFactorViewModel
            {
                HasAuthenticator = hasAuthenticator,
                IsMachineRemembered = isMachineRemembered,
                RecoveryCodesLeft = recoveryCodesLeft
            };

            ApiKeyView = new ApiKeyViewModel
            {
                ApiKey = user.ApiKey
            };

            ChangePasswordInput = new ChangePasswordInputModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            await LoadAsync(user);
            return Page();
        }

        // public async Task<IActionResult> OnPostAsync()
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //     if (user == null)
        //     {
        //         return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //     }

        //     if (!ModelState.IsValid)
        //     {
        //         await LoadAsync(user);
        //         return Page();
        //     }

        //     var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        //     if (Input.PhoneNumber != phoneNumber)
        //     {
        //         var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
        //         if (!setPhoneResult.Succeeded)
        //         {
        //             var userId = await _userManager.GetUserIdAsync(user);
        //             throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
        //         }
        //     }

        //     await _signInManager.RefreshSignInAsync(user);
        //     StatusMessage = "Your profile has been updated";
        //     return RedirectToPage();
        // }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (EmailChangeInput.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, EmailChangeInput.NewEmail);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = EmailChangeInput.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    EmailChangeInput.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                EmailChangeStatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            EmailChangeStatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            EmailChangeStatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTwoFactorForgetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();
            TwoFactorStatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, ChangePasswordInput.OldPassword, ChangePasswordInput.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            ChangePasswordStatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }
    }
}
