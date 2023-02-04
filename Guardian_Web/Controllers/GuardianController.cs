using AutoMapper;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Guardian_Web.Controllers
{
    public class GuardianController : Controller
    {
        private readonly IGuardianService _guardianService;
        private readonly IMapper _mapper;

        public GuardianController(IGuardianService guardianService, IMapper mapper)
        {
            _guardianService = guardianService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexGuardian()
        {
            List<GuardianDTO> list = new();

            var response = await _guardianService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<GuardianDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CreateGuardian()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGuardian(GuardianCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _guardianService.CreateAsync<APIResponse>(model);

                if (response != null && response.IsSuccess)
                {
                    //Redirect back to the index action method that willl reaload al the table informations
                    return RedirectToAction(nameof(IndexGuardian));
                }
            }
            return View(model);
        }
    }
}
