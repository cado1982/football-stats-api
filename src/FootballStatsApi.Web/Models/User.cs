using System;
using Microsoft.AspNetCore.Identity;

namespace FootballStatsApi.Web.Models
{
    public class User : IdentityUser<int>
    {
        [PersonalData]
        public Guid ApiKey { get; set; }
    }
}