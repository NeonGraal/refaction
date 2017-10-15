using System.Collections.Generic;
using refactor_me.Model.Contract;
using System.Linq;
using System;

namespace refactor_me.Model.Implementation
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _repo;

        public ProductsService(IProductsRepository repo)
        {
            _repo = repo;
        }

        public void Create(IProduct product)
        {
            if (_repo.Get(product.Id) == null)
            {
                _repo.Save(product, false);
            }
        }

        public IProductList FindByName(string name)
        {
            var result = new ProductList();

            var ids = _repo.ByName(name);
            foreach (var i in ids)
            {
                result.Add(_repo.Get(i));
            }

            return result;
        }

        public IProduct Get(Guid id)
        {
            return _repo.Get(id);
        }

        public IProductList GetAll()
        {
            var result = new ProductList();

            var ids = _repo.All();
            foreach (var i in ids)
            {
                result.Add(_repo.Get(i));
            }
            return result;
        }

        public void Update(Guid id, IProduct product)
        {
            var existing = _repo.Get(id);

            if (existing != null)
            {
                var updated = new Product(id, product);
                _repo.Save(updated, true);
            }
        }

        private class Product : IProduct
        {
            public Product(Guid id, IProduct product)
            {
                Id = id;
                Name = product.Name;
                Description = product.Description;
                Price = product.Price;
                DeliveryPrice = product.DeliveryPrice;
            }

            public decimal DeliveryPrice { get; }

            public string Description { get; }

            public Guid Id { get; }

            public string Name { get; }

            public decimal Price { get; }
        }

        private class ProductList : IProductList
        {
            private readonly List<IProduct> _items;

            public IEnumerable<IProduct> Items => _items;

            internal void Add(IProduct product)
            {
                _items.Add(product);
            }

            internal ProductList()
            {
                _items = new List<IProduct>();
            }
        }
    }

}