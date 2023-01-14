using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Domain.DTO.GuardianTask
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
    }
}
