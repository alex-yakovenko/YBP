using Sample.Definitions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Definitions.Companies.Dto
{
    public class CompanyInfo: NewCompanyInfo, ITitledEntity
    {
        public int Id { get; set; }
    }

    public class NewCompanyInfo
    {
        public string Title { get; set; }
        public bool IsApproved { get; set; }
    }
}
