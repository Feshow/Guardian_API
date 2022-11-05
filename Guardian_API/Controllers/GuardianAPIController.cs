using Guardian.Application.DTO;
using Guardian.Data;
using Guardian_API.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Guardian_API.Controllers
{
    //If I change the controller name, the route does not change. It is safer in cases that we have many clients using the API.
    [Route("api/GuardianAPI")]
    [ApiController]
    public class GuardianAPIController : ControllerBase
    {
        #region Logger Dependency Injection - Default
        ////Logger Dependency Injection - DEFAULT
        //public ILogger<GuardianAPIController> _logger { get; }
        ////Because of dependency injection, .NET code will provide the implementation of Logger inthe the _logger, so I do not have to instantiate the class or worry about disposing
        //public GuardianAPIController(ILogger<GuardianAPIController> logger)
        //{
        //    _logger = logger;
        //}
        #endregion

        private readonly ILogging _logger;

        public GuardianAPIController(ILogging logger)
        {
            _logger = logger;
        }

        //If you do not define HTTP verb, it defaults to [HttpGet]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //ActionResult is the emplementation of Interface that is used to return the type we want
        public ActionResult<IEnumerable<GuardianDTO>> Get()
        {
            //LOG in console window
            _logger.Log("Getting informations", "");
            //Ok == StatusCode 200 (Success)
            return Ok(GuardianData.guardianList);
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
                _logger.Log($@"Get by ID Error - {id}", "error");
                //BadResquest == StatusCode 400 ()
                return BadRequest();
            }
            var response = GuardianData.guardianList.FirstOrDefault(x => x.Id == id);
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
            if (GuardianData.guardianList.FirstOrDefault(x => x.Name.ToLower() == guardianDTO.Name.ToLower()) != null)
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
            guardianDTO.Id = GuardianData.guardianList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            GuardianData.guardianList.Add(guardianDTO);

            return CreatedAtRoute("Get by Id", new { id = guardianDTO.Id }, guardianDTO); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
        }

        [HttpDelete("{id:int}", Name = "Delete")]
        //Delete does not return any data, so it not necessary to give a return in IActionResult
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var result = GuardianData.guardianList.FirstOrDefault(x => x.Id == id);

            if (result == null)
                return NotFound();
            else
                GuardianData.guardianList.Remove(result);

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
            var objectGuardian = GuardianData.guardianList.FirstOrDefault(x => x.Id == id);
            objectGuardian.Name = guardianDTO.Name;
            objectGuardian.Occupancy = guardianDTO.Occupancy;
            objectGuardian.Description = guardianDTO.Description;

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
            
            var objectGuardian = GuardianData.guardianList.FirstOrDefault(x => x.Id == id);
            if(objectGuardian == null)
                return BadRequest();

            //Patch makes possible to specify the property that we would like to update
            patchDTO.ApplyTo(objectGuardian, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }
    }
}
