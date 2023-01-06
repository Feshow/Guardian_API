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

        //If you do not define HTTP verb, it defaults to [HttpGet]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //ActionResult is the emplementation of Interface that is used to return the type we want
        public ActionResult<IEnumerable<GuardianDTO>> Get()
        {
            //Ok == StatusCode 200 (Success)
            return Ok(_db.Guardians);
        }

        //When you have the same type of verb, it is necessary to give a name to them
        [HttpGet("{id:int}", Name = "Get by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //OTHER WAYS TO DO:
        //[ProducesResponseType(200, Type = typeof(GuardianDTO))] //Define what are the multiple response type that can be produced + It is possible to difine the response type
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<GuardianDTO> GetById(int id)
        {
            if (id == 0)
            {
                //BadResquest == StatusCode 400 ()
                return BadRequest();
            }
            var response = _db.Guardians.FirstOrDefault(x => x.Id == id);
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
        //When you are working with HTTP post, typically the object that you recive is from body
        public ActionResult<GuardianDTO> Create([FromBody] GuardianDTO guardianDTO)
        {
            //ModelState is used to verify if the class rules are being followed (Custom validations witg Data Notatiosn)
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //Creating a custom model state
            if (_db.Guardians.FirstOrDefault(x => x.Name.ToLower() == guardianDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This name already exists!");
                return BadRequest(ModelState);
            }

            if (guardianDTO == null)
            {
                return BadRequest(guardianDTO);
            }
            if (guardianDTO.Id > 0 || guardianDTO.Name == "")
            {
                return BadRequest(StatusCodes.Status500InternalServerError);
            }

            GuardianModel model = new()
            {
                Id = guardianDTO.Id,
                Name = guardianDTO.Name,
                Age = guardianDTO.Age,
                Adress = guardianDTO.Adress,
                Occupancy = guardianDTO.Occupancy,
                CreatedDate = DateTime.Now,
                Status = true
            };

            _db.Guardians.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("Get by Id", new { id = guardianDTO.Id }, guardianDTO); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
        }

        [HttpDelete("{id:int}", Name = "Delete")]
        //Delete does not return any data, so it not necessary to give a return in IActionResult
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var guardian = _db.Guardians.FirstOrDefault(x => x.Id == id);

            if (guardian == null)
                return NotFound();

            _db.Guardians.Remove(guardian);
            _db.SaveChanges();

            return NoContent(); // It could be 'Ok'
        }

        //Used to update the complete object
        [HttpPut("{id:int}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, GuardianDTO guardianDTO)
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
                Adress = guardianDTO.Adress,
                Occupancy = guardianDTO.Occupancy
            };
            _db.Update(model);
            _db.SaveChanges();

            return NoContent(); // It could be 'Ok'
        }
        //Used to update only one specific property
        [HttpPatch("{id:int}", Name = "UpdateProperty")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePatch(int id, JsonPatchDocument<GuardianDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
                return BadRequest();

            //AsNoTracking tell entity framework core that when you retrive this record, you do not want to track that (Avoid ID problem - will not track the record)
            //Every time you are retriving one record EF is alwais tracking that
            var objectGuardian = _db.Guardians.AsNoTracking().FirstOrDefault(x => x.Id == id);

            //It is necessary to convert the model to DTO when we are changing some specific property
            GuardianDTO guardianDTO = new()
            {
                Id = objectGuardian.Id,
                Name = objectGuardian.Name,
                Age = objectGuardian.Age,
                Adress = objectGuardian.Adress,
                Occupancy = objectGuardian.Occupancy
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
                Occupancy = guardianDTO.Occupancy
            };

            //.Updata still updating the whole entity, so if you need to update only one proporty it is necessary go into some store prog and create a store prog to update onw record.
            _db.Guardians.Update(model);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
