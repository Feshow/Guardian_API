using Guardian.Domain.DTO.LoginUser;
using Guardian.Domain.DTO.LoginUser.Registration;
using Guardian.Domain.Interfaces.IRepository.User;
using Guardian.Domain.Models.API;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Guardian_API.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UserController : Controller
    {
        protected APIResponse _response;
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this._response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess= false;
                _response.ErrorMessages.Add("Username or passaword is incorrect");
                return BadRequest(_response);                
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;

            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationResquestDTO model)
        {
            bool uniqueUser = _userRepository.IsUniqueUser(model.UserName);
            if (!uniqueUser)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already existis");
                return BadRequest(_response);
            }

            var newUser = await _userRepository.Register(model);

            if (newUser == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }
    }
}
