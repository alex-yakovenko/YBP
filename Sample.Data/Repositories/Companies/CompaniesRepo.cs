using Sample.Data.Repositories.Base;
using Sample.Data.Entities;
using Sample.Definitions.Companies;
using System.Linq;
using Sample.Definitions.Common;

namespace Sample.Data.Repositories.Companies
{
    public class CompanyRepo : 
        BaseListRepo<Company, CompanyFilter, CompanyOrder, int>,
        ICompanyWriter, 
        ICompanyReader
    {
        protected CompanyRepo(SampleDbContext db) : base(db)
        {
        }

        protected override IQueryable<Company> ApplyFilter(IQueryable<Company> qry, CompanyFilter filter)
        {
            if (filter.IsApproved.HasValue)
                qry = qry.Where(x => x.IsApproved == filter.IsApproved.Value);

            if (!string.IsNullOrEmpty(filter.Title))
                qry = qry.Where(x => x.Title.Contains(filter.Title));

            return base.ApplyFilter(qry, filter);
        }

        protected override IQueryable<Company> ApplyExpression(IQueryable<Company> qry, CompanyOrder order, bool first, bool desc)
        {
            switch (order)
            {
                case CompanyOrder.Title:
                    return qry.OrderFunction(x => x.Title, first, desc);

                default:
                    return base.ApplyExpression(qry, order, first, desc);
            }
        }

    }
}
