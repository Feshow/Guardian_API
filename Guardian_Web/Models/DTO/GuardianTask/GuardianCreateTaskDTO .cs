using System.ComponentModel.DataAnnotations;

namespace Guardian_Web.Models.DTO.GuardianTask
{
    public class GuardianCreateTaskDTO
    {
        [Required]
        public string TaksName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Category { get; set; }
        [Required]
        public int Priority { get; set; }
        public bool Status { get; set; } = true;
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int IdResponsible { get; set; }

    }
}
