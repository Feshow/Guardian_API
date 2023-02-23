using System.ComponentModel.DataAnnotations;

namespace Guardian_Web.Models.DTO.Guardian
{
    public class GuardianCreateDTO
    {

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Occupancy { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
