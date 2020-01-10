using System.Collections.Generic;

namespace FootballStatsApi.Web.Areas.Identity.Models
{
    public class EmailChangeViewModel
    {
        public bool IsEmailConfirmed { get; set; }
        public string Email { get; set; }
    }
}
