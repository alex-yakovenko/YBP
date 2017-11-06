using System;

namespace Sample.Definitions.Common
{
    public interface IRepo<TKey>: 
            IEntitySaver<TKey>,
            IEntityRetriever<TKey>
    {
        void Delete(TKey id);
        BoundItem[] GetBoundItems(TKey id);
    }

    public interface IRepo : IRepo<int>
    {
    };
}
