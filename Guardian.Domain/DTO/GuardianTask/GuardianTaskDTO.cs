using Guardian.Domain.DTO.Guardian;
using System.ComponentModel.DataAnnotations;

namespace Guardian.Domain.DTO.GuardianTask
{
    public class GuardianTaskDTO
    {
        [Required]
        public int Id { get; set; }
        public string TaksName { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int Priority { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required]
        public int IdResponsible { get; set; }
        public GuardianDTO GuardianModel { get; set; } //The property name should be equal to FK property in Model class
    }
}
