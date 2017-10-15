using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Model.Contract;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductsService _products;

        public ProductsController(IProductsService products)
        {
            _products = products;
        }

        [Route]
        [HttpGet]
        public IProductList GetAll()
        {
            return _products.GetAll();
        }

        [Route]
        [HttpGet]
        public IProductList SearchByName(string name)
        {
            return _products.FindByName(name);
        }

        [Route("{id}")]
        [HttpGet]
        public IProduct GetProduct(Guid id)
        {
            var product = _products.Get(id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(JsonProduct product)
        {
            _products.Create(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, JsonProduct product)
        {
            _products.Update(id, product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _products.Delete(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }

        public class JsonProduct : IProduct
        {
            public decimal DeliveryPrice { get; set; }

            public string Description { get; set; }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }
        }
    }
}
