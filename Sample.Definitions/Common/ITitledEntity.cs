namespace Sample.Definitions.Common
{
    public interface ITitledEntity<TKey> : IEntity<TKey>
    {
        string Title { get; set; }
    }
    public interface ITitledEntity : ITitledEntity<int>
    {
    }
}