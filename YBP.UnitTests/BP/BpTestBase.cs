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
using Sample.BP;


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

            c.InitDataContext(connString);
            c.InitYbpDataContext(connString);

            c.InitBLServices();
            InitializeServices(c);

            c.AddSingleton(new YbpUserContext { { "UserId", 75675 } });

            serviceProvider = c.BuildServiceProvider();

            CreateDatabases();
            await CreateRoles();
        }

        public abstract void InitializeServices(ServiceCollection c);

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
