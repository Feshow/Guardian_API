using AutoMapper;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Models.DTO.GuardianTask;
using Guardian_Web.Models.ViewModel;
using Guardian_Web.Services;
using Guardian_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Guardian_Web.Controllers
{
    public class GuardianTaskController : Controller
    {
        private readonly IGuardianTaskService _guardianTaskService;
        private readonly IGuardianService _guardianService;
        private readonly IMapper _mapper;

        public GuardianTaskController(IGuardianTaskService guardianTaskService, IGuardianService guardianService, IMapper mapper)
        {
            _guardianTaskService = guardianTaskService;
            _guardianService = guardianService;
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

        public async Task<IActionResult> CreateTaskGuardian()
        {
            GuardianTaskCreateVM guardianTaskVM = new();

            var response = await _guardianService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                //Populate  the dropdown
                guardianTaskVM.GuardianList = JsonConvert.DeserializeObject<List<GuardianDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(guardianTaskVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTaskGuardian(GuardianTaskCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _guardianTaskService.CreateAsync<APIResponse>(model.Guardian);

                if (response != null && response.IsSuccess)
                {
                    //Redirect back to the index action method that willl reaload al the table informations
                    TempData["success"] = "Updated successfully";
                    return RedirectToAction(nameof(IndexGuardianTask));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            //Populate the dropdaown again if the response is invalid
            var res = await _guardianService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                //Populate  the dropdown
                model.GuardianList = JsonConvert.DeserializeObject<List<GuardianDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            TempData["error"] = "Error encounted.";
            return View(model);
        }

        public async Task<IActionResult> UpdateTaskGuardian(int guardianTaskId)
        {
            GuardianTaskUpdateVM guardianTaskVM = new();

            var response = await _guardianTaskService.GetAsync<APIResponse>(guardianTaskId);
            if (response != null && response.IsSuccess)
            {
                GuardianTaskDTO model = JsonConvert.DeserializeObject<GuardianTaskDTO>(Convert.ToString(response.Result));
                guardianTaskVM.GuardianTask = _mapper.Map<GuardianUpdateTaskDTO>(model);
            }

            response = await _guardianService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                //Populate  the dropdown
                guardianTaskVM.GuardianList = JsonConvert.DeserializeObject<List<GuardianDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                return View(guardianTaskVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTaskGuardian(GuardianTaskUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _guardianTaskService.UpdadeAsync<APIResponse>(model.GuardianTask);

                if (response != null && response.IsSuccess)
                {
                    //Redirect back to the index action method that willl reaload al the table informations
                    TempData["success"] = "Updated successfully";
                    return RedirectToAction(nameof(IndexGuardianTask));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }

            }

            //Populate the dropdaown again if the response is invalid
            var res = await _guardianService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                //Populate  the dropdown
                model.GuardianList = JsonConvert.DeserializeObject<List<GuardianDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            TempData["error"] = "Error encounted.";
            return View(model);
        }

        public async Task<IActionResult> DeleteTaskGuardian(int guardianTaskId)
        {
            var response = await _guardianTaskService.GetAsync<APIResponse>(guardianTaskId);
            if (response != null && response.IsSuccess)
            {
                GuardianTaskDTO model = JsonConvert.DeserializeObject<GuardianTaskDTO>(Convert.ToString(response.Result));

                if (model != null)
                {
                    if (response != null && response.IsSuccess)
                    {
                        return View(model);
                    }
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskGuardian(GuardianTaskDTO model)
        {
            var response = await _guardianTaskService.DeleteAsync<APIResponse>(model.Id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Removed successfully";
                return RedirectToAction(nameof(IndexGuardianTask));
            }
            TempData["error"] = "Error encounted.";
            return View(model);
        }
    }
}
