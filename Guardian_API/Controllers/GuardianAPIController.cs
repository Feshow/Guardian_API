using Guardian.Application.DTO;
using Guardian.Data;
using Microsoft.AspNetCore.Mvc;

namespace Guardian_API.Controllers
{
    //If I change the controller name, the route does not change. It is safer in cases that we have many clients using the API.
    [Route("api/GuardianAPI")]
    [ApiController]
    public class GuardianAPIController : ControllerBase
    {
        //If you do not define HTTP verb, it defaults to [HttpGet]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //ActionResult is the emplementation of Interface that is used to return the type we want
        public ActionResult<IEnumerable<GuardianDTO>> Get()
        {
            //Ok == StatusCode 200 (Success)
            return Ok(GuardianData.guardianList);
        }

        //When you have the same type of verb, it is necessary to give a name to them
        [HttpGet ("{id:int}", Name ="Get by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //OTHER WAYS TO DO:
        //[ProducesResponseType(200, Type = typeof(GuardianDTO))] //Define what are the multiple response type that can be produced + It is possible to difine the response type
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<GuardianDTO> GetById(int id)
        {
            if(id == 0)
            {
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
        public ActionResult<GuardianDTO> Create([FromBody]GuardianDTO guardianDTO)
        {
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

            return CreatedAtRoute("Get by Id", new { id = guardianDTO.Id}, guardianDTO); //After create the object, it gerates the route where we can acesss the objet by id (Invoke GetById);
        }
    }
}
