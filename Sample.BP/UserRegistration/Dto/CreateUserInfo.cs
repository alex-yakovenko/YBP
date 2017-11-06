using Sample.Definitions.Users;

namespace Sample.BP.UserRegistration.Dto
{
    public class CreateUserInfo
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
