using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TechGalaxyProject.Data.Models
{
    public class AppUser:IdentityUser
    {
        public string Role { get; set; }
        public bool IsVerified { get; set; }
        public virtual ICollection<CompletedFields> completedFields { get; set; }
        public virtual ExpertVerificationRequest request { get; set; }
        public virtual ICollection<ExpertVerificationRequest> requests { get; set; }
        public virtual ICollection<Roadmap> createdRoadmaps { get; set; }
        public virtual ICollection<FollowedRoadmap> followedRoadmaps { get; set; }
       // public virtual ICollection<RoadmapLike> RoadmapLikes { get; set; }
    }
}
