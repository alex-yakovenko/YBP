using System;
using System.Threading.Tasks;
using YBP.Framework;

namespace Sample.BP.UserRegistration
{
    public class SendInvitation : YbpAction<UserRegistrationProcess>
    {

        public SendInvitation(IYbpEngine engine) : base(engine)
        {
        }

        protected async override Task<string> RunAsync(YbpContext<UserRegistrationProcess> context, string prm)
        {
            return null;
        }

    }
}
