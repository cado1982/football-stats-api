using System.ComponentModel.DataAnnotations;

namespace FootballStatsApi.Web.Areas.Identity.Models
{
    public class EmailChangeInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}
