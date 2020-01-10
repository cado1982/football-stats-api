using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Web.Models;
using FootballStatsApi.Logic.Managers;
using FootballStatsApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace FootballStatsApi.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ISubscriptionManager subscriptionManager, UserManager<User> userManager)
        {
            _logger = logger;
            _subscriptionManager = subscriptionManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var subscriptions = await _subscriptionManager.GetAllSubscriptions();
            var user = await _userManager.GetUserAsync(User);

            var vm = new IndexViewModel
            {
                Subscriptions = subscriptions,
                User = user
            };

            return View(vm);
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

        [Authorize]
        public async Task<IActionResult> UpgradeSubscription([FromQuery][Required] int subscriptionId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _subscriptionManager.ChangeUsersSubscription(user.Id, subscriptionId);

            return RedirectToAction("Index");
        }
    }
}
