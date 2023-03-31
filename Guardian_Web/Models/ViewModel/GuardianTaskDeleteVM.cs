using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Models.DTO.GuardianTask;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guardian_Web.Models.ViewModel
{
    public class GuardianTaskDeleteVM
    {
        public GuardianTaskDeleteVM()
        {
            GuardianTask = new GuardianUpdateTaskDTO();
        }

        public GuardianUpdateTaskDTO GuardianTask { get; set; }

        [ValidateNever]
        public GuardianDTO Guardian { get; set; }
    }
}
