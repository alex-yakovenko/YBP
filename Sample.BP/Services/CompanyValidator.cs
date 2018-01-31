using Sample.Definitions.Companies;
using Sample.Definitions.Common;
using Sample.Definitions.Companies.Dto;

namespace Sample.Services.Company
{
    public class CompanyValidator : ICompanyValidator
    {
        private readonly ICompanyReader _companyReader;

        public CompanyValidator(ICompanyReader companyReader)
        {
            _companyReader = companyReader;
        }

        public RuleViolations ValidateCompany(CompanyInfo prm)
        {
            var result = new RuleViolations();

            if (string.IsNullOrWhiteSpace(prm.Title))
                result.AddError(nameof(prm.Title), "Please specify company name");
            else
            {

                if (_companyReader
                    .Any(new CompanyFilter
                    {
                        Title = prm.Title,
                        ExceptIds = new[] { prm.Id }
                    }))
                {
                    result.AddError(nameof(prm.Title), "A company with this name already exists");
                }
            }

            return result;
        }
    }
}
