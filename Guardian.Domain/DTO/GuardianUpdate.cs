using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Application.DTO
{
    public class GuardianUpdateDTO
    {
        [Required]
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
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? UpdatedDate { get; set; }
        public bool Status { get; set; }
    }
}
