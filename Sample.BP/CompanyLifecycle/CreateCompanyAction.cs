using Sample.BP.Common;
using Sample.Definitions.Companies;
using Sample.Definitions.Companies.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBP.Framework;
using AutoMapper;

namespace Sample.BP.CompanyLifecycle
{
    public class CreateCompanyAction : YbpFirstAction<CompanyLifecycleProcess, NewCompanyInfo, CreateItemResult>
    {
        private readonly ICompanyValidator _companyValidator;
        private readonly ICompanyWriter _companyWriter;

        public CreateCompanyAction(
            IYbpEngine engine,
            ICompanyValidator companyValidator,
            ICompanyWriter companyWriter
            ) 
            : base(engine)
        {
            _companyValidator = companyValidator;
            _companyWriter = companyWriter;
        }


        protected async override Task<CreateItemResult> RunAsync(YbpContext<CompanyLifecycleProcess> context, NewCompanyInfo prm)
        {
            var result = new CreateItemResult();

            var info = Mapper.Map<CompanyInfo>(prm);

            result.Errors = _companyValidator.ValidateCompany(info);

            if (result.Errors.Any())
                return result;

            result.Id = _companyWriter.Save(info);

            result.Success = true;

            context.Id = result.Id.ToString();

            return result;
        }


        public class MapConfig : AutoMapper.Profile
        {
            public MapConfig()
            {
                CreateMap<NewCompanyInfo, CompanyInfo>();
            }
        }

    }
}
