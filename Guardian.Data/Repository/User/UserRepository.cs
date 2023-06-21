using AutoMapper;
using Guardian.Domain.DTO.LoginUser;
using Guardian.Domain.DTO.LoginUser.Registration;
using Guardian.Domain.Interfaces.IRepository.User;
using Guardian.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Guardian.Data.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager; //Provide helpers functions
        private readonly RoleManager<IdentityRole> _roleManager; //Provide helpers functions
        private string secretKey;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());

            if (user == null)
                return true;

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (user == null || isValid == false)
            {
                LoginResponseDTO loginResponseDTO = new LoginResponseDTO(user: null, token: "");
            }

            //if the user was found it is necessary to generate the JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey); //That will convert the secret key which is in string to byte, then into byte array and finally the key variable
            
            //Token descriptor: Contain the claims how is responsible to "translate" the cripto and defines what the token should contains, how to encrypt, when it does expires asn others properties
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            LoginResponseDTO loginResponse = new LoginResponseDTO(_mapper.Map<UserDTO>(user), stringToken);

            return loginResponse;
        }

        public async Task<UserDTO> Register(RegistrationResquestDTO registrationResquest)
        {
            ApplicationUser user = new()
            {
                UserName = registrationResquest.UserName,
                Email = registrationResquest.UserName,
                NormalizedEmail = registrationResquest.UserName.ToUpper(),
                Name = registrationResquest.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationResquest.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(registrationResquest.Role).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registrationResquest.Role));
                    }
                    await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationResquest.UserName);
                  
                    return _mapper.Map<UserDTO>(user);
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }
    }
}
