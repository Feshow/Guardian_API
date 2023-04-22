using Guardian.Domain.DTO.LoginUser;
using Guardian.Domain.DTO.LoginUser.Registration;
using Guardian.Domain.Interfaces.IRepository.User;
using Guardian.Domain.Models;
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
        private string secretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUser.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());

            if (user == null)
                return true;

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = _db.LocalUser.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower()
                                                    && u.Password == loginRequest.Password);
            if (user == null)
            {
                LoginResponseDTO loginResponseDTO = new LoginResponseDTO(user: null, token: "");
            }


            //if the user was found it is necessary to generate the JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey); //That will convert the secret key which is in string to byte, then into byte array and finally the key variable
            
            //Token descriptor: Contain the claims how is responsible to "translate" the cripto and defines what the token should contains, how to encrypt, when it does expires asn others properties
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            LoginResponseDTO loginResponse = new LoginResponseDTO(user, stringToken);

            return loginResponse;
        }

        public async Task<LocalUser> Register(RegistrationResquestDTO registrationResquest)
        {
            LocalUser user = new LocalUser(registrationResquest.UserName, registrationResquest.Name, registrationResquest.Password, registrationResquest.Role);
            _db.LocalUser.Add(user);
            await _db.SaveChangesAsync();

            user.Password = "";
            return user;
        }
    }
}
