using FootballStatsApi.Domain.Entities.Identity;
using FootballStatsApi.Models.v0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Web.Models
{
    public class IndexViewModel
    {
        public List<Subscription> Subscriptions { get; set; }
        public User User { get; set; }
    }
}
