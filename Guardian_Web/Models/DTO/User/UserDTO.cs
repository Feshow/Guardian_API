using Guardian_Web.Models.DTO.LoginUser.Registration;

namespace Guardian_Web.Models.DTO.User
{
    public class UserDTO
    {
        public UserDTO(string userName, string name, string password)
        {
            UserName = userName;
            Name = name;
            Password = password;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public bool IsValidUser(RegistrationResquestDTO registrationResquest)
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password);
        }
    }
}
