using System;

namespace Sample.Definitions.Common
{
    public class ListFilterBase<TKey>
    {
        public TKey[] Ids { get; set; }

        public TKey[] ExceptIds { get; set; }
    }

}