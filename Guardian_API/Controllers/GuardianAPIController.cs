using Guardian.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Guardian_API.Controllers
{
    [Route("api/GuardianAPI")]
    [ApiController]
    public class GuardianAPIController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<GuardianModel> Get()
        {
            return new List<GuardianModel> {
                new GuardianModel{Id = 1, Name="Felippe Delesporte"},
                new GuardianModel{Id = 2, Name="Enrico Delesporte"}
                };
        }
    }
}
