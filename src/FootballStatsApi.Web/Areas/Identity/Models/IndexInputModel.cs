using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Web.Areas.Identity.Models
{
    public class IndexInputModel
    {
        public ChangePasswordInputModel ChangePassword { get; set; }
        public EmailChangeInputModel EmailChange { get; set; }
        public TwoFactorInputModel TwoFactor { get; set; }
    }
}
