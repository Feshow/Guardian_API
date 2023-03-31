using Guardian_Web.Models.DTO.GuardianTask;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guardian_Web.Models.ViewModel
{
    public class GuardianTaskUpdateVM
    {
        public GuardianTaskUpdateVM()
        {
            GuardianTask = new GuardianUpdateTaskDTO();
        }

        public GuardianUpdateTaskDTO GuardianTask { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> GuardianList { get; set; }
    }
}
