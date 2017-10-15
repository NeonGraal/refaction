using System.Collections.Generic;
using refactor_me.Model.Contract;
using System.Linq;
using System;

namespace refactor_me.Model.Implementation
{
    public class ProductOptionsService : IProductOptionsService
    {
        private readonly IProductOptionsRepository _repo;

        public ProductOptionsService(IProductOptionsRepository repo)
        {
            _repo = repo;
        }

        public void Create(Guid productId, IProductOption option)
        {
            if (_repo.Get(option.Id) == null)
            {
                var toCreate = new ProductOption(option);
                toCreate.ProductId = productId;
                _repo.Save(toCreate, false);
            }
        }

        public void Delete(Guid id)
        {
            var existing = _repo.Get(id);

            if (existing != null)
            {
                _repo.Remove(id);
            }
        }

        public IProductOption Get(Guid id)
        {
            return _repo.Get(id);
        }

        public IProductOptionList GetAll(Guid productId)
        {
            var result = new ProductOptionList();

            var ids = _repo.All(productId);
            foreach (var i in ids)
            {
                result.Add(_repo.Get(i));
            }
            return result;
        }

        public void Update(Guid id, IProductOption option)
        {
            var existing = _repo.Get(id);

            if (existing != null)
            {
                var updated = new ProductOption(option);
                updated.Id = id;
                _repo.Save(updated, true);
            }
        }

        private class ProductOption : IProductOption
        {
            public ProductOption(IProductOption option)
            {
                Id = option.Id;
                ProductId = option.ProductId;
                Name = option.Name;
                Description = option.Description;
            }

            public string Description { get; }

            public Guid Id { get; set; }

            public string Name { get; }

            public Guid ProductId { get; set; }
        }

        private class ProductOptionList : IProductOptionList
        {
            private readonly List<IProductOption> _items;

            public IEnumerable<IProductOption> Items => _items;

            internal void Add(IProductOption option)
            {
                _items.Add(option);
            }

            internal ProductOptionList()
            {
                _items = new List<IProductOption>();
            }
        }
    }

}