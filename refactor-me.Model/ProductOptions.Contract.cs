using System;
using System.Collections.Generic;

namespace refactor_me.Model.Contract
{
    public interface IProductOption
    {
        Guid Id { get; }  
        Guid ProductId { get; }
        string Name { get; }
        string Description { get; }
    }

    public interface IProductOptionList
    {
        IEnumerable<IProductOption> Items { get; }
    }

    public interface IProductOptionsService
    {
        IProductOptionList GetAll(Guid productId);
        IProductOption Get(Guid id);
        void Create(Guid productId, IProductOption option);
        void Update(Guid id, IProductOption option);
        void Delete(Guid id);
    }

    public interface IProductOptionsRepository
    {
        IEnumerable<Guid> All(Guid productId);
        IProductOption Get(Guid id);
        void Save(IProductOption option, bool exists);
        void Remove(Guid id);
    }
}
