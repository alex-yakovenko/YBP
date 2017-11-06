using System;

namespace Sample.Definitions.Common
{
    public interface IEntityRetriever<TKey>
    {
        TEntity Get<TEntity>(TKey id);
    }
}