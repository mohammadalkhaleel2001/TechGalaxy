using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechGalaxyProject.Data.Models
{
    public class Field
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof (roadmap))]
        public int RoadmapId { get; set; }
        public Roadmap roadmap { get; set; }
        public int Order { get; set; }

        public ICollection<CompletedFields> completedFields { get; set; }
    }
}
