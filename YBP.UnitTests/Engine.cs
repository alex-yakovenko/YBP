using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.BP.UserRegistration;
using Sample.BP.UserRegistration.Dto;
using Sample.Data;
using Sample.Definitions;
using Sample.Definitions.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using YBP.Framework;

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

            CreateUserResult r = null;

            using (var tr = dc.Database.BeginTransaction())
                try
                {
                    r = await action.StartAsync(new CreateUserInfo
                    {
                        FirstName = "John",
                        LastName = $"Doe_{Guid.NewGuid()}".Replace("-", ""),
                        Email = "wrong email",
                        Role = SystemRole.RegularUser
                    });

                    tr.Commit();
                }
                catch {

                }

            Assert.IsTrue(r.Success);
        }

        [TestMethod]
        public void CreateDb()
        {
            var dc = new SampleDbContext();
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
    }
}
