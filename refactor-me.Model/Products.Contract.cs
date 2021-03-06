﻿using System;
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
        IEnumerable<IProduct> Items { get; }
    }

    public interface IProductsService
    {
        IProductList GetAll();
        IProductList FindByName(string name);
        IProduct Get(Guid id);
        void Create(IProduct product);
        void Update(Guid id, IProduct product);
        void Delete(Guid id);
    }

    public interface IProductsRepository
    {
        IEnumerable<Guid> All();
        IEnumerable<Guid> ByName(string name);
        IProduct Get(Guid id);
        void Save(IProduct product, bool exists);
        void Remove(Guid id);
    }
}
