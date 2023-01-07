using AutoMapper;
using Guardian.Application.DTO;
using Guardian.Application.Interfaces.IRepository;
using Guardian.Data;
using Guardian.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Guardian_API.Controllers
{
    //If I change the controller name, the route does not change. It is safer in cases that we have many clients using the API.
    [Route("api/GuardianAPI")]
    [ApiController]
    public class GuardianAPIController : ControllerBase
    {
        private readonly IGuardianRepository _dbGuardian;
        private readonly IMapper _mapper;

        //Appling Dependency Injection
        public GuardianAPIController(IGuardianRepository dbGuardian, IMapper mapper)
        {
            _dbGuardian = dbGuardian;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GuardianDTO>>> GetAll()
        {
            IEnumerable<GuardianModel> guardianList = await _dbGuardian.GetAllAsync();
            return Ok(_mapper.Map<List<GuardianDTO>>(guardianList));
        }

        [HttpGet("{id:int}", Name = "Get by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GuardianDTO>> GetById(int id)
        {
            if (id == 0)            
                return BadRequest();            
            
            var response = await _dbGuardian.GetAsync(x => x.Id == id);
            
            if (response == null)           
                return NotFound();
            
            return Ok(_mapper.Map<GuardianDTO>(response));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GuardianDTO>> Create([FromBody] GuardianCreateDTO createDTO)
        {
            if (await _dbGuardian.GetAsync(x => x.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This name already exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            GuardianModel model = _mapper.Map<GuardianModel>(createDTO);

            await _dbGuardian.CreateAsync(model);
            return CreatedAtRoute("Get by Id", new { id = model.Id }, model); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
        }

        [HttpDelete("{id:int}", Name = "Delete")]
        //Delete does not return any data, so it not necessary to give a return in IActionResult
        public async Task<IActionResult>Delete(int id)
        {
            if (id == 0)
                return BadRequest();


            var guardian = await _dbGuardian.GetAsync(x => x.Id == id);

            if (guardian == null)
                return NotFound();

            await _dbGuardian.RemoveAsync(guardian);
            return NoContent();
        }

        //Used to update the complete object
        [HttpPut("{id:int}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, GuardianUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            
            GuardianModel model = _mapper.Map<GuardianModel>(updateDTO);

            await _dbGuardian.UpdateAsync(model);
            return NoContent(); // It could be 'Ok'
        }

        //Used to update only one specific property
        [HttpPatch("{id:int}", Name = "UpdateProperty")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePatch(int id, JsonPatchDocument<GuardianUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
                return BadRequest();

            //AsNoTracking tell entity framework core that when you retrive this record, you do not want to track that (Avoid ID problem - will not track the record)
            //Every time you are retriving one record EF is alwais tracking that
            var objectGuardian = await _dbGuardian.GetAsync(x => x.Id == id, tracked: false);
            if (objectGuardian == null)
                return BadRequest();

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
            
            return NoContent();
        }
    }
}
