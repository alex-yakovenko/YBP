using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sample.Definitions.Users
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(
            IUserStore<AppUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<AppUser> passwordHasher, 
            IEnumerable<IUserValidator<AppUser>> userValidators, 
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<AppUser>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }

    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(
            IRoleStore<AppRole> store, 
            IEnumerable<IRoleValidator<AppRole>> roleValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            ILogger<RoleManager<AppRole>> logger) 
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }
    }
}
