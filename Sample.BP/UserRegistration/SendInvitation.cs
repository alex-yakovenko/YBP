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

        public override Func<YbpFlagsDictionary, bool> NeedsToBeExecuted => 
            flags => flags[UserRegistrationProcess.Flags.NeedSendInvitation];

        public override Func<YbpFlagsDictionary, bool> CannotToBeExecuted =>
            flags => flags.AlreadyExecuted<SendInvitation>();

        protected async override Task<string> RunAsync(YbpContext<UserRegistrationProcess> context, string prm)
        {
            return null;
        }

    }
}
