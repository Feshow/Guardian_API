using Guardian_Web.Models.DTO.Guardian;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian_Web.Models.DTO.GuardianTask
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
        public GuardianDTO GuardianModel { get; set; }
    }
}
