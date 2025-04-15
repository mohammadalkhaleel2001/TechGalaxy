using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechGalaxyProject.Data.Models
{
    public class ExpertVerificationRequest
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [ForeignKey(nameof(User))]
        public int ReviewedBy { get; set; }
        public AppUser User { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string status { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string AdminNote { get; set; }
    }
}
