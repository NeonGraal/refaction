using System.Collections.Generic;
using refactor_me.Model.Contract;
using System.Linq;

namespace refactor_me.Model.Implementation
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _repo;

        public ProductsService(IProductsRepository repo)
        {
            _repo = repo;
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