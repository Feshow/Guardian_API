using AutoMapper;
using Guardian_Utility;
using Guardian_Web.Models;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Guardian_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGuardianService _guardianService;
        private readonly IMapper _mapper;

        public HomeController(IGuardianService guardianService, IMapper mapper)
        {
            _guardianService = guardianService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<GuardianDTO> list = new();

            var response = await _guardianService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<GuardianDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
    }
}