using Sample.Data.Entities;
using Sample.Definitions.Companies.Dto;


namespace Sample.Data.Repositories.Companies
{
    public class MapConfig : AutoMapper.Profile
    {
        public MapConfig()
        {
            CreateMap<Company, CompanyInfo>();
            CreateMap<CompanyInfo, Company>().MapOnlyIfDirty();
        }
    }
}