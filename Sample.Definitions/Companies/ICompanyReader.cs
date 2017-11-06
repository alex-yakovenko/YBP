using Sample.Definitions.Common;

namespace Sample.Definitions.Companies
{
    public interface ICompanyReader: IListSource<CompanyFilter, CompanyOrder, int>
    {
    }

    public class CompanyFilter: ListFilterBase<int>
    {
        public string Title { get; set; }
        public bool? IsApproved { get; set; }
    }

    public enum CompanyOrder
    {
        Title
    }
}
