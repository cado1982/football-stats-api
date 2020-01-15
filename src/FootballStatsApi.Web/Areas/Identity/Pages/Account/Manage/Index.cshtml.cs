using System.Collections.Generic;
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
using FootballStatsApi.Web.Areas.Identity.Models;
using FootballStatsApi.Logic.v0.Managers;
using IndexViewModel = FootballStatsApi.Web.Areas.Identity.Models.IndexViewModel;
using System.Web;

namespace FootballStatsApi.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            IEmailSender emailSender,
            UserManager<User> userManager,
            ILogger<IndexModel> logger,
            ISubscriptionManager subscriptionManager,
            SignInManager<User> signInManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _logger = logger;
            _subscriptionManager = subscriptionManager;
            _signInManager = signInManager;
        }

        public IndexViewModel ViewModel { get; set; }

        [BindProperty]
        public IndexInputModel InputModel { get; set; }

        [TempData]
        public string EmailChangeStatusMessage { get; set; }

        [TempData]
        public string EmailChangeErrorMessage { get; set; }

        [TempData]
        public string TwoFactorStatusMessage { get; set; }

        [TempData]
        public string ChangePasswordStatusMessage { get; set; }

        [TempData]
        public string ChangePasswordErrorMessage { get; set; }

        private async Task LoadAsync(User user)
        {
            var email = await _userManager.GetEmailAsync(user);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            var hasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            var is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var isMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            var recoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
            var subscription = await _subscriptionManager.GetSubscriptionById(user.SubscriptionId);

            InputModel = new IndexInputModel
            {
                EmailChange = new EmailChangeInputModel
                {
                    NewEmail = email,
                },
                ChangePassword = new ChangePasswordInputModel(),
                TwoFactor = new TwoFactorInputModel
                {
                    Is2faEnabled = is2faEnabled
                }
            };

            ViewModel = new IndexViewModel
            {
                ApiKey = new ApiKeyViewModel
                {
                    ApiKey = user.ApiKey
                },
                EmailChange = new EmailChangeViewModel
                {
                    Email = email,
                    IsEmailConfirmed = isEmailConfirmed
                },
                TwoFactor = new TwoFactorViewModel
                {
                    HasAuthenticator = hasAuthenticator,
                    IsMachineRemembered = isMachineRemembered,
                    RecoveryCodesLeft = recoveryCodesLeft
                },
                Subscription = new SubscriptionViewModel
                {
                    ActiveSubscription = subscription
                },
                ChangePassword = new ChangePasswordViewModel()
            };
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
                return RedirectToPage("Index", "ChangeEmail", "ChangeEmail");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (InputModel.EmailChange.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, InputModel.EmailChange.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = InputModel.EmailChange.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    InputModel.EmailChange.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                EmailChangeStatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage("Index", "ChangeEmail", "ChangeEmail");
            }

            EmailChangeStatusMessage = "Your email is unchanged.";
            return RedirectToPage("Index", "ChangeEmail", "ChangeEmail");
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
                return RedirectToPage("Index", "SendVerificationEmail", "ChangeEmail");
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
            return RedirectToPage("Index", "SendVerificationEmail", "ChangeEmail");
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
            return RedirectToPage("Index", "TwoFactor", "TwoFactor");
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
                return RedirectToPage("Index", "ChangePassword", "ChangePassword");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, InputModel.ChangePassword.OldPassword, InputModel.ChangePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                ChangePasswordErrorMessage = string.Join("\n", changePasswordResult.Errors.Select(e => e.Description));
                await LoadAsync(user);
                return RedirectToPage("Index", "ChangePassword", "ChangePassword");
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            ChangePasswordStatusMessage = "Your password has been changed.";

            return RedirectToPage("Index", "ChangePassword", "ChangePassword");
        }
    }
}
