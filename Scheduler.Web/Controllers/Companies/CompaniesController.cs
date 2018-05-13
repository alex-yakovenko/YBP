using Microsoft.AspNetCore.Mvc;
using Sample.BP.Common;
using Sample.BP.CompanyLifecycle;
using Sample.Definitions.Companies;
using Sample.Definitions.Companies.Dto;
using System.Threading.Tasks;

namespace Scheduler.Web.Controllers.Companies
{
    [Route("api/[controller]/[action]/{id?}")]
    public class CompaniesController: Controller
    {
        private readonly ICompanyReader _companyReader;
        private readonly CreateCompanyAction _createCompanyAction;
        private readonly UpdateCompanyAction _updateCompanyAction;

        public CompaniesController(
            ICompanyReader companyReader,
            CreateCompanyAction createCompanyAction,
            UpdateCompanyAction updateCompanyAction
            )
        {
            _companyReader = companyReader;
            _createCompanyAction = createCompanyAction;
            _updateCompanyAction = updateCompanyAction;
        }

        [HttpGet]
        public CompaniesListModel List(CompanyFilter filter)
        {
            var model = new CompaniesListModel {

                TotalCount = _companyReader.GetCount(filter),
                Items = _companyReader.GetList<CompanyInfo>(filter)
            };

            return model;
        }

        [HttpGet]
        public CompanyDetailsModel Details(int id)
        {
            var model = new CompanyDetailsModel
            {
                Data = id != 0
                ? _companyReader
                    .GetFirst<CompanyInfo>(new CompanyFilter
                    {
                        Ids = new[] { id }
                    })
                : new CompanyInfo
                {

                }
            };

            return model;
        }

        [HttpPost]
        public async Task<SaveItemResult> Save([FromBody] CompanyInfo data)
        {

            if (data.Id == 0)
            {
                return await _createCompanyAction.StartAsync(data);

            } else
            {
                return await _updateCompanyAction.ExecAsync(data.Id.ToString(), data);
            }

        }

    }
}
