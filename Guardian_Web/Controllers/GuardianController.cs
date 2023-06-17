using AutoMapper;
using Guardian_Utility;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

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

            var response = await _guardianService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<GuardianDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateGuardian()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGuardian(GuardianCreateDTO model)
        {
            model.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                var response = await _guardianService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));

                if (response != null && response.IsSuccess)
                {
                    //Redirect back to the index action method that willl reaload al the table informations
                    TempData["success"] = "Registred successfully"; //Sweet alert
                    return RedirectToAction(nameof(IndexGuardian));
                }
            }
            TempData["error"] = "Error encounted.";
            return View(model);
        }
        
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateGuardian(int guardianId)
        {
            var response = await _guardianService.GetAsync<APIResponse>(guardianId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                GuardianDTO model = JsonConvert.DeserializeObject<GuardianDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<GuardianUpdateDTO>(model));
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGuardian(GuardianUpdateDTO model)
        {
            var getGuardian = await _guardianService.GetAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));

            if (getGuardian != null && getGuardian.IsSuccess)
            {
                GuardianDTO guardian = JsonConvert.DeserializeObject<GuardianDTO>(Convert.ToString(getGuardian.Result));
                model.CreatedDate = guardian.CreatedDate;
                model.UpdatedDate = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                var response = await _guardianService.UpdadeAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));

                if (response != null && response.IsSuccess)
                {
                    //Redirect back to the index action method that willl reaload al the table informations
                    TempData["success"] = "Updated successfully"; //Sweet alert
                    return RedirectToAction(nameof(IndexGuardian));
                }
            }
            TempData["error"] = "Error encounted.";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteGuardian(int guardianId)
        {
            var response = await _guardianService.GetAsync<APIResponse>(guardianId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                GuardianDTO model = JsonConvert.DeserializeObject<GuardianDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGuardian(GuardianDTO model)
        {
            var response = await _guardianService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Removed successfully";
                return RedirectToAction(nameof(IndexGuardian));
            }
            TempData["error"] = "Error encounted.";
            return View(model);
        }
    }
}
