using Guardian_Web.Models.DTO.GuardianTask;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guardian_Web.Models.ViewModel
{
    public class GuardianTaskCreateVM
    {
        public GuardianTaskCreateVM()
        {
            Guardian = new GuardianCreateTaskDTO();
        }

        public GuardianCreateTaskDTO Guardian { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> GuardianList { get; set; }
    }
}
