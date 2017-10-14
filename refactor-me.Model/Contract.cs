using System;

namespace refactor_me.Model.Contract
{
    public interface Product
    {
        Guid Id { get; }

        string Name { get; }

        string Description { get; }

        decimal Price { get; }

        decimal DeliveryPrice { get; }
    }

    public interface ProductOption
    {
        Guid Id { get; }

        Guid ProductId { get; }

        string Name { get; }

        string Description { get; }
    }
}
