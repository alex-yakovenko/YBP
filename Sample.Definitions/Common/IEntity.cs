using System;

namespace Sample.Definitions.Common
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IEntity: IEntity<int>
    {
    }

    public interface ICheckoutId
    {
        int CheckoutId { get; set; }
    }

    public class CheckoutIdInfo : ICheckoutId
    {
        public int CheckoutId { get; set; }
        public bool Allowed { get; set; }
    }
}
