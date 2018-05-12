using Sample.Definitions.Companies.Dto;

namespace Scheduler.Web.Controllers.Companies
{
    public class CompaniesListModel
    {
        public int TotalCount { get; set; }
        public CompanyInfo[] Items { get; set; }
    }
}
