using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechGalaxyProject.Data.Models
{
    public class CompletedFields
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int LearnerId { get; set; }
        public AppUser User { get; set; }
        [ForeignKey(nameof (field))]
        public int FieldId { get; set; }
        public Field field { get; set; }
        public DateTime CompletedAt { get; set; }

    }
}
