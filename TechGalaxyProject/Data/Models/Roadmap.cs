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
        public int CreatedBy { get; set; }
        public AppUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Field> fields { get; set; }
        public ICollection<FollowedRoadmap> followedBy { get; set; }
    }
}
