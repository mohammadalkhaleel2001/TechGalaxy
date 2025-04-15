using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TechGalaxyProject.Data.Models
{
    public class AppUser:IdentityUser
    {
        public string Role { get; set; }
        public bool IsVerified { get; set; }
        public ICollection<CompletedFields> completedFields { get; set; }
        public ExpertVerificationRequest request { get; set; }
        public ICollection<ExpertVerificationRequest> requests { get; set; }
        public ICollection<Roadmap> createdRoadmaps { get; set; }
        public ICollection<FollowedRoadmap> followedRoadmaps { get; set; }
    }
}
