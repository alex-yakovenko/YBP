using YBP.Framework;

namespace Sample.BP.CompanyLifecycle
{
    public class CompanyLifecycleProcess: YbpProcessBase
    {
        public CompanyLifecycleProcess()
        {
            Actions = new IYbpActionDefinition[] {
                new YbpFirstActionDefinition<CreateCompanyAction>(),
                new YbpActionDefinition<UpdateCompanyAction>()
            };
        }
    }
}
