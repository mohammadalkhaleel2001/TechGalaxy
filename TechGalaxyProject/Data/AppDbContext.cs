using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechGalaxyProject.Data.Models;

namespace TechGalaxyProject.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CompletedFields> completedFields { get; set; }
        public DbSet<ExpertVerificationRequest> ExpertVerificationRequests { get; set; }
        public DbSet<Field> fields { get; set; }
        public DbSet<Roadmap> roadmaps { get; set; }
        public DbSet<FollowedRoadmap> FollowedRoadmaps { get; set; }
    }
}
