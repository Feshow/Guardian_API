using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guardian.Domain.Models
{
    public class GuardianTaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("GuardianModel")]
        public int IdResponsible { get; set; }
        public GuardianModel GuardianModel { get; set; }
        public string TaksName { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int Priority { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public void Inativar(GuardianTaskModel model)
        {
            model.Status = false;
            model.UpdatedDate = DateTime.Now;
        }
    }
}