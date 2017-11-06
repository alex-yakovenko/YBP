using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Data;
using Sample.Definitions;
using System.Threading.Tasks;
using System.Transactions;

namespace YBP.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private ServiceProvider serviceProvider;
        private UserManager<AppUser> userManager;

        [TestInitialize]
        public void Setup()
        {
            var c = new ServiceCollection();
            c.AddLogging()
                .AddDbContext<SampleDbContext>()
                .AddIdentity<AppUser, AppRole>()
                .AddUserStore<UserStore<AppUser, AppRole, SampleDbContext, int>>();

            //    .AddSingleton<IFooService, FooService>()
            //    .AddSingleton<IBarService, BarService>()
              serviceProvider =  c.BuildServiceProvider();

            userManager = serviceProvider.GetService<UserManager<AppUser>>();

        }

        [TestMethod]
        public async Task TestMethod1()
        {

            var usr = await userManager.CreateAsync(new AppUser
            {
                UserName = "Alex"
            });

            Assert.IsTrue(usr.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUser()
        {
            using (var tr = new TransactionScope())
            {

                var user = await userManager.FindByNameAsync("alex");
                user.Email = "uemon2005@gmail.com";

                await userManager.UpdateAsync(user);
            }
        }


        [TestMethod]
        public void RunContext()
        {
            var ctx = new SampleDbContext();

            ctx.Database.EnsureCreated();
        }

    }
}
