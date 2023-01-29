using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian_Web.Models.DTO.GuardianTask
{
    public class GuardianUpdateTaskDTO
    {
        [Required]
        public int Id { get; set; }
        public string TaksName { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int Priority { get; set; }        
        public bool Status { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [Required]
        public int IdResponsible { get; set; }
    }
}
