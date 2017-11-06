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

namespace YBP.UnitTests
{
    [TestClass]
    public class Engine
    {
        public IYbpEngine _bp;
        private ServiceProvider serviceProvider;


        [TestInitialize]
        public void Init()
        {
            var c = new ServiceCollection();
            c.AddLogging()
                .AddTransient<IYbpEngine, YbpEngine>()
                .AddTransient<IYbpContextStorage, YbpContextStorage>()
                .AddTransient<CreateUserAction>()
                .AddTransient<AppUserManager>()
                .AddTransient<AppRoleManager>()
                .AddSingleton(new YbpUserContext { {"UserId", 75675 } })
                .AddDbContext<SampleDbContext>()
                .AddIdentity<AppUser, AppRole>()
           
                .AddUserStore<UserStore<AppUser, AppRole, SampleDbContext, int>>()
                .AddRoleStore<RoleStore<AppRole, SampleDbContext, int>>();

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
            var dc = new SampleDbContext();
            dc.Database.EnsureCreated();
        }

        [TestMethod]
        public void CreateStorageDb()
        {
            var dc = new YbpDbContext();
            dc.Database.EnsureCreated();
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
    }
}
