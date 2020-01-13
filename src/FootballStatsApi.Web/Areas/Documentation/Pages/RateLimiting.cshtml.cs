using System.Threading.Tasks;
using FootballStatsApi.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Logic.Managers;
using FootballStatsApi.Models;
using System.Collections.Generic;
using FootballStatsApi.Domain.Entities.Identity;

namespace FootballStatsApi.Web.Areas.Documentation.Pages
{
    public class RateLimitingModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ILogger<RateLimitingModel> _logger;

        public List<Subscription> Subscriptions { get; set; }

        public string ApiKey { get; set; }

        public RateLimitingModel(
            UserManager<User> userManager,
            ISubscriptionManager subscriptionManager,
            ILogger<RateLimitingModel> logger)
        {
            _userManager = userManager;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}