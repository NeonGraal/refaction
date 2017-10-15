using System;
using System.Collections.Generic;

namespace refactor_me.Model.Contract
{
    public interface IProduct
    {
        Guid Id { get; }

        string Name { get; }

        string Description { get; }

        decimal Price { get; }

        decimal DeliveryPrice { get; }
    }

    public interface IProductList
    {
        ICollection<IProduct> Items { get; }
    }

    public interface IProductOption
    {
        Guid Id { get; }

        Guid ProductId { get; }

        string Name { get; }

        string Description { get; }
    }

    public interface IProducts
    {
        IProductList GetAll();
    }
}
