using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.BP.CompanyLifecycle;

namespace YBP.UnitTests.BP.Companies
{
    [TestClass]
    public class UpdateCompany: BpTestBase
    {
        public override void InitializeServices(ServiceCollection c)
        {
            c.AddTransient<UpdateCompanyAction>();
        }

        public void UpdateUser()
        {
            var target = serviceProvider.GetService<UpdateCompanyAction>();

        }

    }
}
