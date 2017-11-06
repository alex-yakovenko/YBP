namespace Sample.Definitions.Common
{
    public interface IEntitySaver<TKey>
    {
        TKey Save<TUpdateInfo>(TUpdateInfo info) 
            where TUpdateInfo : class, IEntity<TKey>, new();
    }

    public interface IEntitySaver : IEntitySaver<int>
    {
    }
}