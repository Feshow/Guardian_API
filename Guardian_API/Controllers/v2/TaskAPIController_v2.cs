using AutoMapper;
using Guardian.Domain.Interfaces.IRepository.Guardian;
using Guardian.Domain.Interfaces.IRepository.TaskGuardian;
using Guardian.Domain.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace Guardian_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/TaskAPI")]
    [ApiController]
    [ApiVersion("2.0", Deprecated = true)] //Deprecated is displayed on swagger documentation to mke the developers aware about it
    public class TaskAPIController_v2 : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGuardianTaskRepository _dbTask;
        private readonly IGuardianRepository _dbGuardian;
        private readonly IMapper _mapper;

        public TaskAPIController_v2(IGuardianTaskRepository dbTask, IMapper mapper, IGuardianRepository guardianRepository)
        {
            _dbTask = dbTask;
            _mapper = mapper;
            _response = new();
            _dbGuardian = guardianRepository;
        }

        [HttpGet("GetString")]
        //[MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
