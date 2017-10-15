using System.Collections.Generic;
using refactor_me.Model.Contract;

namespace refactor_me.Model.Implementation
{
    public class Products : IProducts
    {
        public IProductList GetAll()
        {
            return new ProductList();
        }

        private class ProductList : IProductList
        {
            public ICollection<IProduct> Items { get; }

            internal ProductList()
            {
                Items = new List<IProduct>();
            }
        }
    }

}