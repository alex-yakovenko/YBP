using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Data;
using Sample.Data.Repositories.Companies;
using Sample.Definitions;
using Sample.Definitions.Companies;
using System.Threading.Tasks;
using System.Transactions;
using Sample.Definitions.Companies.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace YBP.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private ServiceProvider serviceProvider;
        private UserManager<AppUser> userManager;

        private ICompanyWriter companyWriter;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets("YBP-B24A7B7F-D538-4230-9AEB-11928B687712")
                .Build();

            var ybpSampleConnectionString = config["YbpSampleAppConnectionString"];


            var c = new ServiceCollection();
            c.AddLogging()
                .AddTransient<ICompanyReader, CompanyRepo>()
                .AddTransient<ICompanyWriter, CompanyRepo>()
                .AddDbContext<SampleDbContext>(opt => opt.UseSqlServer(ybpSampleConnectionString))
                .AddIdentity<AppUser, AppRole>()
                .AddUserStore<UserStore<AppUser, AppRole, SampleDbContext, int>>();

              serviceProvider =  c.BuildServiceProvider();

            Mapper.Initialize(cfg => cfg.AddProfiles(new[] { typeof(CompanyRepo).Assembly }));

            userManager = serviceProvider.GetService<UserManager<AppUser>>();
            companyWriter = serviceProvider.GetService<ICompanyWriter>();
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

        [TestMethod]
        public void CreateCompany()
        {
            companyWriter.Save(new CompanyInfo
            {
                Title = $"Company {Guid.NewGuid()}",
                IsApproved = true
            });
        }

    }
}
