using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.BP.CompanyLifecycle;
using Sample.BP.UserRegistration;
using Sample.Data;
using Sample.Definitions;
using Sample.Definitions.Companies;
using Sample.Definitions.Companies.Dto;
using Sample.Definitions.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBP.Framework;
using YBP.Framework.Regisry;
using YBP.Framework.Storage.EF;

namespace YBP.UnitTests.BP.Companies
{
    [TestClass]
    public class CreateCompany: BpTestBase
    {

        public override void InitializeServices(ServiceCollection c)
        {
            base.InitializeServices(c);

            c.AddTransient<CreateCompanyAction>();
        }


        [TestMethod]
        public async Task TestCreateCompany()
        {
            var action = serviceProvider.GetService<CreateCompanyAction>();
            var prm = new NewCompanyInfo {
                Title = null                
            };

            var r = await action.StartAsync(prm);

            Assert.IsFalse(r.Success);
            Assert.AreEqual("Please specify company name", r.Errors["Title"][0]);

            prm.Title = "New company";
            prm.IsApproved = true;

            r = await action.StartAsync(prm);

            Assert.IsTrue(r.Success);
            Assert.IsFalse(r.Errors.Any());
            Assert.AreNotEqual(0, r.Id);

            var companyReader = serviceProvider.GetService<ICompanyReader>();
            var company = companyReader.GetFirst<CompanyInfo>(new CompanyFilter
            {
                Ids = new[] { r.Id }
            });

            Assert.IsNotNull(company);
            Assert.AreEqual(prm.Title, company.Title);
            Assert.AreEqual(prm.IsApproved, company.IsApproved);
        }

        [TestMethod]
        public void Run2()
        {
            TestContext.WriteLine(connString);
            Assert.IsTrue(true);
        }

    }
}
