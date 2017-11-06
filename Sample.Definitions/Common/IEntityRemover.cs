using System;

namespace Sample.Definitions.Common
{
    public interface IEntityRemover<TKey>
    {
        void Delete(TKey id);
        CanDeleteCheckResult CheckCanDelete(TKey id);
    }

    public interface IEntityRemover : IEntityRemover<int>
    {
    }

    public class DeleteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class CanDeleteCheckResult
    {
        public bool CanDelete { get; set; }
        public string CantDeleteMessage { get; set; }
        public string WarningMessage { get; set; }
    }

    public class BoundItem : ITitledEntity
    {
       // public EntityType EntityType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }
}