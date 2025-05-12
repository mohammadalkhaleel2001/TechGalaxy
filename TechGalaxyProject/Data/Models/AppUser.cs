using Microsoft.AspNetCore.Identity;

namespace TechGalaxyProject.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; } = default!;
        public bool IsVerified { get; set; }

        // ✅ NEW: Specialty only for Expert role
        public string? Specialty { get; set; }

        // ✅ NEW: Certificate file path (for verification)
        public string? CertificatePath { get; set; }

        // Relations
        public virtual ICollection<CompletedFields> completedFields { get; set; } = new List<CompletedFields>();
        public virtual ExpertVerificationRequest? request { get; set; }
        public virtual ICollection<ExpertVerificationRequest> requests { get; set; } = new List<ExpertVerificationRequest>();
        public virtual ICollection<Roadmap> createdRoadmaps { get; set; } = new List<Roadmap>();
        public virtual ICollection<FollowedRoadmap> followedRoadmaps { get; set; } = new List<FollowedRoadmap>();
    }
}
