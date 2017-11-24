using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.BP.UserRegistration;
using Sample.Data;
using Sample.Data.Repositories.Companies;
using Sample.Definitions;
using Sample.Definitions.Companies;
using Sample.Definitions.Users;
using Sample.Services.Company;
using System;
using System.Threading.Tasks;
using YBP.Framework;
using YBP.Framework.Regisry;
using YBP.Framework.Storage.EF;
using AutoMapper;


namespace YBP.UnitTests.BP
{
    public abstract class BpTestBase
    {
        public ServiceProvider serviceProvider;
        public string connString;
        public string dbName;
        public string serverName;
        public TestContext TestContext { get; set; }

        public BpTestBase()
        {
            YbpConfiguration.LoadActionsFromAssembly<CreateUserAction>();
            Mapper.Initialize(c => {
                c.AddProfiles(typeof(SampleDbContext).Assembly); // Sample.Data
                c.AddProfiles(typeof(CreateUserAction).Assembly); // Sample.BP
                c.AddProfiles(typeof(ICompanyValidator).Assembly); // Sample.Definitions
                c.AddProfiles(typeof(CompanyValidator).Assembly); // SampleServices
            });
        }



        [TestInitialize]
        public async Task Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("bptestsettings.json")
                .Build();

            serverName = config["TestSqlServer"];
            var key = TestContext.TestName;
            var timestamp = DateTime.Now.ToString("MMdd_hhmmss");
            dbName = $"t_{timestamp}_{key}";
            connString = $"Server={serverName};Database={dbName};Trusted_Connection=True;";

            var c = new ServiceCollection();
            c.AddLogging()
                .AddTransient<IYbpEngine, YbpEngine>()
                .AddTransient<IYbpContextStorage, YbpContextStorage>()
                .AddTransient<AppUserManager>()
                .AddTransient<AppRoleManager>();

            InitializeServices(c);

            c.AddSingleton(new YbpUserContext { { "UserId", 75675 } })

                .AddDbContext<SampleDbContext>(opt => opt.UseSqlServer(connString))
                .AddDbContext<YbpDbContext>(opt => opt.UseSqlServer(connString, x => x.MigrationsHistoryTable("__YbpMigrationsHistory")))
                .AddIdentity<AppUser, AppRole>()

                .AddUserStore<UserStore<AppUser, AppRole, SampleDbContext, int>>()
                .AddRoleStore<RoleStore<AppRole, SampleDbContext, int>>();

            serviceProvider = c.BuildServiceProvider();

            CreateDatabases();
            await CreateRoles();
        }

        public virtual void InitializeServices(ServiceCollection c)
        {
            c.AddTransient<ICompanyReader, CompanyRepo>();
            c.AddTransient<ICompanyWriter, CompanyRepo>();
            c.AddTransient<ICompanyValidator, CompanyValidator>();
        }

        public virtual void CreateDatabases()
        {
            var dc = serviceProvider.GetService<SampleDbContext>();
            if (!dc.Database.EnsureCreated())
                dc.Database.Migrate();

            var ydc = serviceProvider.GetService<YbpDbContext>();
            if (!ydc.Database.EnsureCreated())
                ydc.Database.Migrate();
        }

        public virtual async Task CreateRoles()
        {
            var m = serviceProvider
                .GetService<AppRoleManager>();

            await m.CreateAsync(new AppRole
            {
                Name = SystemRole.Superviser,
                NormalizedName = SystemRole.Superviser.ToUpper()
            });

            await m.CreateAsync(new AppRole
            {
                Name = SystemRole.Admin,
                NormalizedName = SystemRole.Admin.ToUpper()
            });

            await m.CreateAsync(new AppRole
            {
                Name = SystemRole.RegularUser,
                NormalizedName = SystemRole.RegularUser.ToUpper()
            });
        }

        [TestCleanup]
        public void Done()
        {
            var dc = serviceProvider.GetService<SampleDbContext>();
            dc.Database.EnsureDeleted();
        }

    }
}
