using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Web.Areas.Identity.Models
{
    public class IndexViewModel
    {
        public ApiKeyViewModel ApiKey { get; set; }
        public EmailChangeViewModel EmailChange { get; set; }
        public TwoFactorViewModel TwoFactor { get; set; }
        public SubscriptionViewModel Subscription { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }
    }
}
