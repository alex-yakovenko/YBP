using Sample.Definitions.Common;
using Sample.Definitions.Companies.Dto;

namespace Sample.Definitions.Companies
{
    public interface ICompanyValidator
    {
        RuleViolations ValidateCompany(CompanyInfo info);
    }
}
