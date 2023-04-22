using Guardian.Domain.DTO.LoginUser.Registration;

namespace Guardian.Domain.Models
{
    public class LocalUser
    {
        public LocalUser(string userName, string name, string password, string role)
        {
            UserName = userName;
            Name = name;
            Password = password;
            Role = role;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public bool IsValidUser(RegistrationResquestDTO registrationResquest)
        {
           return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Role); 
        }
    }
}
