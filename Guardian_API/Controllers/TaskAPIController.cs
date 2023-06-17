using AutoMapper;
using Guardian.Domain.Interfaces.IRepository.Guardian;
using Guardian.Domain.Interfaces.IRepository.TaskGuardian;
using Guardian.Domain.DTO.GuardianTask;
using Guardian.Domain.Models;
using Guardian.Domain.Models.API;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Guardian_API.Controllers
{
    [Route("api/TaskAPI")]
    [ApiController]
    public class TaskAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGuardianTaskRepository _dbTask;
        private readonly IGuardianRepository _dbGuardian;
        private readonly IMapper _mapper;

        public TaskAPIController(IGuardianTaskRepository dbTask, IMapper mapper, IGuardianRepository guardianRepository)
        {
            _dbTask = dbTask;
            _mapper = mapper;
            this._response = new();
            _dbGuardian = guardianRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllTasks()
        {
            try
            {
                IEnumerable<GuardianTaskModel> taskList = await _dbTask.GetAllAsync(x => x.Status == true, includeProperties: "GuardianModel"); //includeProperties should match with ctor include in repository
                _response.Result = _mapper.Map<List<GuardianTaskDTO>>(taskList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "Get task by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTaskById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var response = await _dbTask.GetAsync(x => x.Id == id && x.Status == true, includeProperties: "GuardianModel");

                if (response == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<GuardianTaskDTO>(response);
                _response.StatusCode = HttpStatusCode.OK;
                return (_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateTask([FromBody] GuardianCreateTaskDTO createTaskDTO)
        {
            try
            {
                if (createTaskDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                if (!createTaskDTO.IsValid(createTaskDTO))
                {
                    ModelState.AddModelError("ErrorMessages", "Task should have title, description and responsible!");
                    return BadRequest(ModelState);
                }

                if (await _dbGuardian.GetAsync(x => x.Id == createTaskDTO.IdResponsible) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Responsible does not exist!");
                    return BadRequest(ModelState);
                }

                GuardianTaskModel model = _mapper.Map<GuardianTaskModel>(createTaskDTO);

                await _dbTask.CreateAsync(model);

                _response.Result = _mapper.Map<GuardianTaskDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("Get task by Id", new { id = model.Id }, _response); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };                
            }
            return (_response);
        }

        [HttpDelete("{id:int}", Name = "Delete task")]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteTask(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var task = await _dbTask.GetAsync(x => x.Id == id);
                if (task == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }
                await _dbTask.UpdateInactivateAsync(task);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "Update task")]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateTask(int id, [FromBody] GuardianUpdateTaskDTO updateTaskDTO)
        {
            try
            {
                if (updateTaskDTO == null || id != updateTaskDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                if (await _dbGuardian.GetAsync(x => x.Id == updateTaskDTO.IdResponsible) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Responsible is invalid!");
                    return BadRequest(ModelState);
                }

                var task = await _dbTask.GetAsync(x => x.Id == id, tracked: true);
                if (task == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }
                else
                {
                    task.TaksName = updateTaskDTO.TaksName;
                    task.Description = updateTaskDTO.Description;
                    task.Category = updateTaskDTO.Category;
                    task.Priority= updateTaskDTO.Priority;
                    task.IdResponsible = updateTaskDTO.IdResponsible;
                    task.UpdatedDate= updateTaskDTO.UpdatedDate;
                }
               
                await _dbTask.UpdateAsync(task);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdateTaskProperty")]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePatchTask(int id, JsonPatchDocument<GuardianUpdateTaskDTO> patchTaskDTO)
        {
            try
            {
                if (patchTaskDTO == null || id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(ModelState);
                }

                var objectTask = await _dbTask.GetAsync(x => x.Id == id, tracked: false);

                if (objectTask == null)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                GuardianUpdateTaskDTO taskDTO = _mapper.Map<GuardianUpdateTaskDTO>(objectTask);

                patchTaskDTO.ApplyTo(taskDTO, ModelState);

                if (await _dbGuardian.GetAsync(x => x.Id == taskDTO.IdResponsible) == null)
                {
                    ModelState.AddModelError("CustomError", "IdResponsible is invalid!");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                GuardianTaskModel model = _mapper.Map<GuardianTaskModel>(taskDTO);
                model.CreatedDate = objectTask.CreatedDate;

                await _dbTask.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
