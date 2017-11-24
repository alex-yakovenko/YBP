using Sample.Definitions.Companies.Dto;
using YBP.Framework;
using System.Threading.Tasks;
using System.Linq;
using Sample.Definitions.Common;
using Sample.Definitions.Companies;

namespace Sample.BP.CompanyLifecycle
{
    public class UpdateCompanyAction : YbpAction<CompanyLifecycleProcess, CompanyInfo, RuleViolations>
    {
        private readonly ICompanyValidator _companyValidator;
        private readonly ICompanyWriter _companyWriter;


        public UpdateCompanyAction(
            IYbpEngine engine,
            ICompanyValidator companyValidator,
            ICompanyWriter companyWriter
            ) 
            : base(engine)
        {
            _companyValidator = companyValidator;
            _companyWriter = companyWriter;
        }

        protected async override Task<RuleViolations> RunAsync(YbpContext<CompanyLifecycleProcess> context, CompanyInfo prm)
        {
            var result = _companyValidator.ValidateCompany(prm);

            if (result.Any())
                return result;

            _companyWriter.Save(prm);

            return result;
        }
    }
}
