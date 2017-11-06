namespace Sample.Definitions.Common
{
    public class TitledListFilter<TKey> : ListFilterBase<TKey>
    {
        public string Title { get; set; }

        public string TitleExact { get; set; }
    }
}