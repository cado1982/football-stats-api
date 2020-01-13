using System.Threading.Tasks;
using FootballStatsApi.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Domain.Entities.Identity;
using FootballStatsApi.Logic.Managers;
using FootballStatsApi.Models;
using System.Collections.Generic;

namespace FootballStatsApi.Web.Areas.Documentation.Pages
{
    public class IntroductionModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ILogger<IntroductionModel> _logger;

        public IntroductionModel(
            UserManager<User> userManager,
            ISubscriptionManager subscriptionManager,
            ILogger<IntroductionModel> logger)
        {
            _userManager = userManager;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
        }
    }
}