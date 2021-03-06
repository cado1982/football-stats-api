using System;
using Microsoft.AspNetCore.Identity;

namespace FootballStatsApi.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        [PersonalData]
        public Guid ApiKey { get; set; }

        public int SubscriptionId { get; set; }
    }
}