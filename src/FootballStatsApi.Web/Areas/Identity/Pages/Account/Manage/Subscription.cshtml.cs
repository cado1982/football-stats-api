using System.Threading.Tasks;
using FootballStatsApi.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Domain.Entities.Identity;
using FootballStatsApi.Logic.v0.Managers;
using FootballStatsApi.Models.v0;
using System.Collections.Generic;

namespace FootballStatsApi.Web.Areas.Identity.Pages.Account.Manage
{
    public class SubscriptionModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ILogger<SubscriptionModel> _logger;

        public List<Subscription> Subscriptions { get; set; }

        public SubscriptionModel(
            UserManager<User> userManager,
            ISubscriptionManager subscriptionManager,
            ILogger<SubscriptionModel> logger)
        {
            _userManager = userManager;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return NotFound();

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //Subscriptions = await _subscriptionManager.GetAllSubscriptions();

            //return Page();
        }
    }
}