using AutoMapper;
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
            c.AddTransient<CreateCompanyAction>();
            c.AddTransient<UpdateCompanyAction>();
        }


        [TestMethod]
        public async Task TestCreateCompany()
        {
            var createAction = serviceProvider.GetService<CreateCompanyAction>();
            var updateAction = serviceProvider.GetService<UpdateCompanyAction>();
            var companyReader = serviceProvider.GetService<ICompanyReader>();


            // Incorrect parameters: empty title
            var prm = new NewCompanyInfo {
                Title = null                
            };

            var r = await createAction.StartAsync(prm);

            Assert.IsFalse(r.Success);
            Assert.AreEqual("Please specify company name", r.Errors["Title"][0]);


            // Correct parameters
            prm.Title = "New company";
            prm.IsApproved = true;

            r = await createAction.StartAsync(prm);

            Assert.IsTrue(r.Success);
            Assert.IsFalse(r.Errors.Any());
            Assert.AreNotEqual(0, r.Id);

            // Retrieve newly created company
            var company = companyReader.GetFirst<CompanyInfo>(new CompanyFilter
            {
                Ids = new[] { r.Id }
            });

            Assert.IsNotNull(company);
            Assert.AreEqual(prm.Title, company.Title);
            Assert.AreEqual(prm.IsApproved, company.IsApproved);

            // Update the company with wrong title

            var updateInfo = Mapper.Map<CompanyInfo>(company);

            updateInfo.Title = " ";

            var r1 = await updateAction.ExecAsync(r.Id.ToString(), updateInfo);

            Assert.IsFalse(r1.Success);
            Assert.AreEqual("Please specify company name", r1.Errors["Title"][0]);

            // Ensure we didn't change db entry
            company = companyReader.GetFirst<CompanyInfo>(new CompanyFilter
            {
                Ids = new[] { r.Id }
            });
            Assert.AreEqual(prm.Title, company.Title);


            // Update the company with correct title
            updateInfo.Title = "New company name";
            updateInfo.IsApproved = false;

            r1 = await updateAction.ExecAsync(r.Id.ToString(), updateInfo);

            Assert.IsTrue(r1.Success);
            Assert.AreEqual(r.Id, r1.Id);

            // Retrieve updated company
            company = companyReader.GetFirst<CompanyInfo>(new CompanyFilter
            {
                Ids = new[] { r.Id }
            });
            Assert.AreEqual(updateInfo.Title, company.Title);
            Assert.AreEqual(updateInfo.IsApproved, company.IsApproved);

        }

        [TestMethod]
        public void Run2()
        {
            TestContext.WriteLine(connString);
            Assert.IsTrue(true);
        }

    }
}
