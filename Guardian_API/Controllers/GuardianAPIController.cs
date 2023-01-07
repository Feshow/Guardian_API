using Guardian.Application.DTO;
using Guardian.Data;
using Guardian.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guardian_API.Controllers
{
    //If I change the controller name, the route does not change. It is safer in cases that we have many clients using the API.
    [Route("api/GuardianAPI")]
    [ApiController]
    public class GuardianAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        //Appling Dependency Injection
        public GuardianAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GuardianDTO>>> Get()
        {
            return Ok(await _db.Guardians.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "Get by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GuardianDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var response = await _db.Guardians.FirstOrDefaultAsync(x => x.Id == id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GuardianDTO>> Create([FromBody] GuardianCreateDTO guardianDTO)
        {
            //ModelState is used to verify if the class rules are being followed (Custom validations witg Data Notation)
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //Creating a custom model state
            if (await _db.Guardians.FirstOrDefaultAsync(x => x.Name.ToLower() == guardianDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This name already exists!");
                return BadRequest(ModelState);
            }

            if (guardianDTO == null)
            {
                return BadRequest(guardianDTO);
            }

            GuardianModel model = new()
            {
                Name = guardianDTO.Name,
                Age = guardianDTO.Age,
                Occupancy = guardianDTO.Occupancy,
                Adress = guardianDTO.Adress,
                CreatedDate = guardianDTO.CreatedDate,
                Status = guardianDTO.Status
            };

            await _db.Guardians.AddAsync(model);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("Get by Id", new { id = model.Id }, model); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
        }

        [HttpDelete("{id:int}", Name = "Delete")]
        //Delete does not return any data, so it not necessary to give a return in IActionResult
        public async Task<IActionResult>Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var guardian = await _db.Guardians.FirstOrDefaultAsync(x => x.Id == id);

            if (guardian == null)
                return NotFound();

            _db.Guardians.Remove(guardian);
            await _db.SaveChangesAsync();

            return NoContent(); // It could be 'Ok'
        }

        //Used to update the complete object
        [HttpPut("{id:int}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, GuardianUpdateDTO guardianDTO)
        {
            if (guardianDTO == null || id != guardianDTO.Id)
            {
                return BadRequest();
            }

            GuardianModel model = new()
            {
                Id = guardianDTO.Id,
                Name = guardianDTO.Name,
                Age = guardianDTO.Age,
                Occupancy = guardianDTO.Occupancy,                
                Adress = guardianDTO.Adress,
                UpdatedDate = guardianDTO.UpdatedDate,
                Status = guardianDTO.Status
            };
            _db.Update(model);
            await _db.SaveChangesAsync();

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
            var objectGuardian = await _db.Guardians.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            //It is necessary to convert the model to DTO when we are changing some specific property
            GuardianUpdateDTO guardianDTO = new()
            {
                Id = objectGuardian.Id,
                Name = objectGuardian.Name,
                Age = objectGuardian.Age,
                Occupancy = objectGuardian.Occupancy,
                Adress = objectGuardian.Adress,
                //UpdatedDate = objectGuardian.UpdatedDate,
                UpdatedDate = DateTime.Now,
                Status = objectGuardian.Status

            };

            if (objectGuardian == null)
                return BadRequest();

            //Patch makes possible to specify the property that we would like to update
            patchDTO.ApplyTo(guardianDTO, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //It is necessary to convert GuardianDTO to model before update de changes
            GuardianModel model = new()
            {
                Id = guardianDTO.Id,
                Name = guardianDTO.Name,
                Age = guardianDTO.Age,
                Adress = guardianDTO.Adress,
                Occupancy = guardianDTO.Occupancy,
                UpdatedDate = guardianDTO.UpdatedDate,
                Status = guardianDTO.Status
            };

            //.Updata still updating the whole entity, so if you need to update only one proporty it is necessary go into some store prog and create a store prog to update onw record.
            _db.Guardians.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
