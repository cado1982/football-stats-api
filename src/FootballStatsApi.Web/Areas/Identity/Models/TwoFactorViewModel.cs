namespace FootballStatsApi.Web.Areas.Identity.Models
{
    public class TwoFactorViewModel
    {
        public bool HasAuthenticator { get; set; }
        public int RecoveryCodesLeft { get; set; }
        public bool IsMachineRemembered { get; set; }
    }
}
