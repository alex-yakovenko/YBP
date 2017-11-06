using Microsoft.AspNetCore.Identity;
using System;

namespace Sample.Definitions
{
    public class AppUser: IdentityUser<int>
    {
    }

    public class AppRole : IdentityRole<int>
    {

    }
}
