using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Data;
using Sample.Data.Repositories.Companies;
using Sample.Definitions;
using Sample.Definitions.Companies;
using Sample.Definitions.Users;
using Sample.Services.Company;
using System;
using YBP.Framework;
using YBP.Framework.Storage.EF;

namespace Sample.BP
{
    public static class Configuration
    {
        public static void InitBLServices(this IServiceCollection c)
        {
            c.AddTransient<ICompanyReader, CompanyRepo>();
            c.AddTransient<ICompanyWriter, CompanyRepo>();
            c.AddTransient<ICompanyValidator, CompanyValidator>();

            c.AddTransient<AppUserManager>()
                .AddTransient<AppRoleManager>();

            c.AddTransient<IYbpEngine, YbpEngine>()
                .AddTransient<IYbpContextStorage, YbpContextStorage>();

            c.AddIdentity<AppUser, AppRole>()
                .AddUserStore<UserStore<AppUser, AppRole, SampleDbContext, int>>()
                .AddRoleStore<RoleStore<AppRole, SampleDbContext, int>>();

        }

        public static void InitDataContext(this IServiceCollection c, string connectionString)
        {
            c.AddDbContext<SampleDbContext>(opt => opt.UseSqlServer(connectionString));
        }

        public static void InitYbpDataContext(this IServiceCollection c, string connectionString)
        {
            c.AddDbContext<YbpDbContext>(opt => opt.UseSqlServer(connectionString));
        }
    }
}
