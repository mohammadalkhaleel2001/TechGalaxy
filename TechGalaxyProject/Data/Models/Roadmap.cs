using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechGalaxyProject.Data.Models
{
    public class Roadmap
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(User))]
        public string CreatedBy { get; set; }
        public virtual AppUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Field> fields { get; set; }
        public virtual ICollection<FollowedRoadmap> followedBy { get; set; }
       // public virtual ICollection<RoadmapLike> RoadmapLikes { get; set; }
    }
}
