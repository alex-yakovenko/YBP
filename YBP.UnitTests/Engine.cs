using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sample.BP.UserRegistration;
using Sample.BP.UserRegistration.Dto;
using Sample.Data;
using Sample.Definitions;
using Sample.Definitions.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using YBP.Framework;
using YBP.Framework.Storage.EF;
using YBP.Framework.Regisry;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Sample.Definitions.Companies;
using Sample.Services.Company;
using Sample.BP;
using Sample.Definitions.Companies.Dto;

namespace YBP.UnitTests
{
    [TestClass]
    public class Engine
    {
        public Engine()
        {
            YbpConfiguration.LoadActionsFromAssembly<CreateUserAction>();
            Mapper.Initialize(c => {
                c.AddProfiles(typeof(SampleDbContext).Assembly); // Sample.Data
                c.AddProfiles(typeof(CreateUserAction).Assembly); // Sample.BP
                c.AddProfiles(typeof(ICompanyValidator).Assembly); // Sample.Definitions
                c.AddProfiles(typeof(CompanyValidator).Assembly); // SampleServices
            });

        }

        public IYbpEngine _bp;
        private ServiceProvider serviceProvider;


        [TestInitialize]
        public void Init()
        {

            var config = new ConfigurationBuilder()
                .AddUserSecrets(Constants.SecretKey)
                .Build();

            var ybpConnectionString = config["YbpConnectionString"];
            var ybpSampleConnectionString = config["YbpSampleAppConnectionString"];

            var c = new ServiceCollection();
            c.AddLogging();
            c.InitDataContext(ybpSampleConnectionString);
            c.InitYbpDataContext(ybpConnectionString);

            c.InitBLServices();

            c.AddSingleton(new YbpUserContext { { "UserId", 75675 } });

            var t = c.ToList();
            serviceProvider = c.BuildServiceProvider();

            _bp = serviceProvider.GetService<IYbpEngine>();
        }


        [TestMethod]
        public async Task RunAsyncAction()
        {
            var action = serviceProvider.GetService<CreateUserAction>();
            var dc = serviceProvider.GetService<SampleDbContext>();

            var r = await action.StartAsync(new CreateUserInfo
                    {
                        FirstName = "John",
                        LastName = $"Doe_{Guid.NewGuid()}".Replace("-", ""),
                        Email = "wrong email",
                        Role = SystemRole.RegularUser
                    });

            Assert.IsTrue(r.Success);
        }

        [TestMethod]
        public void CreateDb()
        {
            var dc = serviceProvider.GetService<SampleDbContext>(); 
            if (!dc.Database.EnsureCreated())
                dc.Database.Migrate();
        }

        [TestMethod]
        public void CreateStorageDb()
        {
            var dc = serviceProvider.GetService<YbpDbContext>();
            if (!dc.Database.EnsureCreated())
                dc.Database.Migrate();
        }

        [TestMethod]
        public async Task CreateRoles()
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

        [TestMethod]
        public void DictSerialize()
        {
            var dict = new Dictionary<string, object> {
                { "UserId", 6456 },
                { "UserRole", "Manager" },
                { "Created", new DateTime(2017, 10, 22, 14, 15, 03) }
            };

            var s = JsonConvert.SerializeObject(dict);

            var d1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);

            Assert.AreEqual(3, d1.Count);

        }

        [TestMethod]
        public void CreateCompany()
        {
            var c = serviceProvider
                .GetService<ICompanyWriter>();

            c.Save(new CompanyInfo
            {
                IsApproved = true,
                Title = "Company 1"
            });

            c.Save(new CompanyInfo
            {
                IsApproved = true,
                Title = "Company 2"
            });


            c.Save(new CompanyInfo
            {
                IsApproved = true,
                Title = "Company 3"
            });

        }
    }
}
