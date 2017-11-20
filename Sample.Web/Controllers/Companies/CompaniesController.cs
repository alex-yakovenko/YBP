using Microsoft.AspNetCore.Mvc;
using Sample.Definitions.Companies;
using Sample.Definitions.Companies.Dto;

namespace Sample.Web.Controllers.Companies
{
    public class CompaniesController: Controller
    {
        private readonly ICompanyReader _companyReader;

        public CompaniesController(ICompanyReader companyReader)
        {
            _companyReader = companyReader;
        }

        [Route("api/companies/list")]
        public CompaniesListModel List()
        {
            var model = new CompaniesListModel {

                TotalCount = _companyReader.GetCount(),
                Items = _companyReader.GetList<CompanyInfo>()
            };

            return model;
        }
    }
}
