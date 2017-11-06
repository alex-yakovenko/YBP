using System;

namespace Sample.Definitions.Common
{
    public class TitledEntity : ITitledEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }

    public class TitledEntity<T>
    {
        public T Id { get; set; }
        public string Title { get; set; }

    }

    public class CheckedTitledEntity : CheckedTitledEntity<int>
    {
        
    }

    public class CheckedTitledEntity<T> : TitledEntity<T>
    {
        public bool Checked { get; set; }
    }
}
