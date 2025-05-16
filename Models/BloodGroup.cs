using System.ComponentModel.DataAnnotations;

namespace bloodsociety.Models
{
    public class BloodGroup
    {
        [Key]
        public int BloodGroupId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}