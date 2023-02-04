using System.ComponentModel.DataAnnotations;

namespace Guardian_Web.Models.DTO.Guardian
{
    public class GuardianDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Occupancy { get; set; }
        [Required]
        public string Adress { get; set; }
    }
}
