using System.ComponentModel.DataAnnotations;

namespace TechGalaxyProject.Models
{
    public class dtoNewUser
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
        public string phonNember { get; set; }
        [Required ]
        public string Role { get; set; }
    }
}
