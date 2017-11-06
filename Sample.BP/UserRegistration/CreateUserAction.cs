using Sample.BP.UserRegistration.Dto;
using Sample.Definitions;
using Sample.Definitions.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBP.Framework;

namespace Sample.BP.UserRegistration
{
    public class CreateUserAction : YbpFirstAction<UserRegistrationProcess, CreateUserInfo, CreateUserResult>
    {
        private readonly AppUserManager _userManager;

        public CreateUserAction(IYbpEngine engine, AppUserManager userManager) : base(engine)
        {
            _userManager = userManager;
        }


        protected override async Task<CreateUserResult> RunAsync(YbpContext<UserRegistrationProcess> context, CreateUserInfo prm)
        {
            var user = new AppUser
            {
                Email = prm.Email,
                UserName = $"{prm.FirstName}_{prm.LastName}".Trim(),
                PhoneNumber = prm.PhoneNumber
            };

            var result = new CreateUserResult {
                Errors = new List<string>()
            };

            foreach (var v in _userManager.UserValidators)
            {
                var ir = await v.ValidateAsync(_userManager, user);

                if (ir.Succeeded)
                    continue;

                result.Errors.AddRange(ir.Errors.Select(x => x.Description));
            }

            if (result.Errors.Any())
                return result;

            var u = await _userManager
                .CreateAsync(user);

            if (!u.Succeeded)
                return result;
            
            result.UserId = user.Id;
            result.Success = true;
            context.Id = $"{user.Id}";
            await _userManager.AddToRoleAsync(user, prm.Role);

            if (prm.Role == SystemRole.RegularUser)
                context.Flags[UserRegistrationProcess.Flags.NeedSendInvitation] = true;

            return result;
        }
    }
}
