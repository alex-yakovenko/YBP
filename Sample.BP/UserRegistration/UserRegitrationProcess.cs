using System;
using System.Collections.Generic;
using YBP.Framework;

namespace Sample.BP.UserRegistration
{
    public class  UserRegistrationProcess: YbpProcessBase
    {
        public override string Prefix => "UsrReg";
        public override string Title => "User registration";

        public class Flags
        {
            public const string NeedSendInvitation = "Need Invitation";
        }

        public UserRegistrationProcess()
        {
            Actions = new IYbpActionDefinition[]
            {
                new YbpFirstActionDefinition<CreateUserAction>(),

                new YbpAutomaticActionDefinition<SendInvitation>
                {
                    NeedsToBeExecuted = flags => flags[Flags.NeedSendInvitation],
                    MayNotBeExecuted = flags => flags.AlreadyExecuted<SendInvitation>()
                }
            };
        }
    }
}
