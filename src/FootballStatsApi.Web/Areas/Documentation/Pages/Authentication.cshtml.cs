using System.Threading.Tasks;
using FootballStatsApi.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Logic.v0.Managers;
using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using FootballStatsApi.Domain.Entities.Identity;

namespace FootballStatsApi.Web.Areas.Documentation.Pages
{
    public class AuthenticationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ILogger<AuthenticationModel> _logger;

        public List<Subscription> Subscriptions { get; set; }

        public string ApiKey { get; set; }

        public AuthenticationModel(
            UserManager<User> userManager,
            ISubscriptionManager subscriptionManager,
            ILogger<AuthenticationModel> logger)
        {
            _userManager = userManager;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(base.User);

            if (user != null)
            {
                ApiKey = user.ApiKey.ToString();
            }

            return Page();
        }
    }
}