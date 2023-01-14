using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Domain.DTO.Guardian
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
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
