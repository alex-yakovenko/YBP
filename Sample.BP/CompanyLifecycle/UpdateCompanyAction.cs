using Sample.Definitions.Companies.Dto;
using YBP.Framework;
using System.Threading.Tasks;
using System.Linq;
using Sample.Definitions.Common;
using Sample.Definitions.Companies;
using Sample.BP.Common;

namespace Sample.BP.CompanyLifecycle
{
    public class UpdateCompanyAction : YbpAction<CompanyLifecycleProcess, CompanyInfo, SaveItemResult>
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

        protected async override Task<SaveItemResult> RunAsync(YbpContext<CompanyLifecycleProcess> context, CompanyInfo prm)
        {
            var result = new SaveItemResult
            {
                Errors = _companyValidator.ValidateCompany(prm)
            };
                
            if (result.Errors.Any())
                return result;

            result.Id = _companyWriter.Save(prm);

            result.Success = true;

            return result;
        }
    }
}
