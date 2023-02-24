using AutoMapper;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.GuardianTask;
using Guardian_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Guardian_Web.Controllers
{
    public class GuardianTaskController : Controller
    {
        private readonly IGuardianTaskService _guardianTaskService;
        private readonly IMapper _mapper;

        public GuardianTaskController(IGuardianTaskService guardianTaskService, IMapper mapper)
        {
            _guardianTaskService = guardianTaskService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexGuardianTask()
        {
            List<GuardianTaskDTO> list = new();

            var response = await _guardianTaskService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<GuardianTaskDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
    }
}
