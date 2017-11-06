using System;
using System.Collections.Generic;
using YBP.Framework;

namespace Sample.BP.UserRegistration
{
    public class  UserRegistrationProcess: YbpProcessBase
    {
        public override string Prefix => "UsrReg";
        public override string Title => "User registration";

        public List<IYbpActionBase> Actions { get; private set; }

        public class Flags
        {
            public const string NeedSendInvitation = "Need Invitation";
        }
    }
}
