using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechGalaxyProject.Data.Models
{
    public class ExpertVerificationRequest
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Expert))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Admin))]
        public string ReviewedBy { get; set; }
        public virtual AppUser Expert { get; set; }

        public virtual AppUser Admin { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string status { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string AdminNote { get; set; }
    }
}
