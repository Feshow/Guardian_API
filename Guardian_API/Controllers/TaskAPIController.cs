using AutoMapper;
using Guardian.Application.Interfaces.IRepository.TaskGuardian;
using Guardian.Domain.DTO.GuardianTask;
using Guardian.Domain.Models;
using Guardian.Domain.Models.API;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Guardian_API.Controllers
{
    [Route("api/TaskAPI")]
    [ApiController]
    public class TaskAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGuardianTaskRepository _dbTask;
        private readonly IMapper _mapper;

        public TaskAPIController(IGuardianTaskRepository dbTask, IMapper mapper)
        {
            _dbTask = dbTask;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllTasks()
        {
            try
            {
                IEnumerable<GuardianTaskModel> taskList = await _dbTask.GetAllAsync();
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

                var response = await _dbTask.GetAsync(x => x.Id == id);

                if (response == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<GuardianCreateTaskDTO>(response);
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
            return(_response);
        }

        [HttpDelete("{id:int}", Name = "Delete task")]
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
    }
}
