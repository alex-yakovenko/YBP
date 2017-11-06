using System.Collections.Generic;

namespace Sample.BP.UserRegistration.Dto
{
    public class CreateUserResult
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public List<string> Errors { get; set; }
    }
}
