using AutoMapper;
using Guardian.Domain.Interfaces.IRepository.Guardian;
using Guardian.Domain.DTO.Guardian;
using Guardian.Domain.Models;
using Guardian.Domain.Models.API;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Guardian_API.Controllers.v1
{
    //If I change the controller name, the route does not change. It is safer in cases that we have many clients using the API.
    [Route("api/v{version:apiVersion}/GuardianAPI")]
    [ApiController]
    [ApiVersion("1.0")] //It is the API avaliable version 
    public class GuardianAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGuardianRepository _dbGuardian;
        private readonly IMapper _mapper;

        //Appling Dependency Injection
        public GuardianAPIController(IGuardianRepository dbGuardian, IMapper mapper)
        {
            _dbGuardian = dbGuardian;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            try
            {
                IEnumerable<GuardianModel> guardianList = await _dbGuardian.GetAllAsync();
                _response.Result = _mapper.Map<List<GuardianDTO>>(guardianList);
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

        [HttpGet("{id:int}", Name = "Get by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var response = await _dbGuardian.GetAsync(x => x.Id == id);

                if (response == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }

                _response.Result = _mapper.Map<GuardianDTO>(response);
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

        [HttpPost]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] GuardianCreateDTO createDTO)
        {
            try
            {
                if (await _dbGuardian.GetAsync(x => x.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "This name already exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }

                GuardianModel model = _mapper.Map<GuardianModel>(createDTO);

                await _dbGuardian.CreateAsync(model);

                _response.Result = _mapper.Map<GuardianDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("Get by Id", new { id = model.Id }, _response); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "Delete")]
        [Authorize(Roles = "admin")]//Only with authorization
        //Delete does not return any data, so it not necessary to give a return in IActionResult
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var guardian = await _dbGuardian.GetAsync(x => x.Id == id);

                if (guardian == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }

                await _dbGuardian.RemoveAsync(guardian);
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

        //Used to update the complete object
        [HttpPut("{id:int}", Name = "Update")]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] GuardianUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var guardian = await _dbGuardian.GetAsync(x => x.Id == id, tracked: true);
                if (guardian == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound();
                }
                else
                {
                    guardian.UpdateGuardian(updateDTO);
                }

                await _dbGuardian.UpdateAsync(guardian);
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

        //Used to update only one specific property
        [HttpPatch("{id:int}", Name = "UpdateProperty")]
        [Authorize(Roles = "admin")]//Only with authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePatch(int id, JsonPatchDocument<GuardianUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                //AsNoTracking tell entity framework core that when you retrive this record, you do not want to track that (Avoid ID problem - will not track the record)
                //Every time you are retriving one record EF is alwais tracking that
                var objectGuardian = await _dbGuardian.GetAsync(x => x.Id == id, tracked: false);
                if (objectGuardian == null)
                {
                    _response.StatusCode = HttpStatusCode.BadGateway;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                //It is necessary to convert the model to DTO when we are changing some specific property
                GuardianUpdateDTO guardianDTO = _mapper.Map<GuardianUpdateDTO>(objectGuardian);

                //Patch makes possible to specify the property that we would like to update
                patchDTO.ApplyTo(guardianDTO, ModelState);

                //It is necessary to convert GuardianDTO to model before update de changes
                GuardianModel model = _mapper.Map<GuardianModel>(guardianDTO);

                //.Updata still updating the whole entity, so if you need to update only one proporty it is necessary go into some store prog and create a store prog to update onw record.
                await _dbGuardian.UpdateAsync(model);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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
