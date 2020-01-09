using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Web.Models;
using FootballStatsApi.Logic.Managers;

namespace FootballStatsApi.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISubscriptionManager _subscriptionManager;

        public HomeController(ILogger<HomeController> logger, ISubscriptionManager subscriptionManager)
        {
            _logger = logger;
            _subscriptionManager = subscriptionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Subscribe(string plan)
        {
            var subscription = await _subscriptionManager.GetSubscriptionByName(plan);

            if (subscription == null) return NotFound();

            var redirectUri = $"/Identity/Account/Register?plan={subscription.InternalName}";

            return Redirect(redirectUri);
        }
    }
}
