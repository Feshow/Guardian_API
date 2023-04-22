using Guardian.Domain.Models;

namespace Guardian.Domain.DTO.LoginUser
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(LocalUser user, string token)
        {
            User= user;
            Token= token;
        }

        public LocalUser User { get; set; }
        public string Token { get; set; } //Token is used to authenticate or rather than validade de identity of that user
    }
}
