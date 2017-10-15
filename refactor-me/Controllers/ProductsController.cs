using System;
using System.Net;
using System.Web.Http;
using refactor_me.Model.Contract;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductsService _products;
        private readonly IProductOptionsService _options;

        public ProductsController(IProductsService products, IProductOptionsService options)
        {
            _products = products;
            _options = options;
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
        public IProductOptionList GetOptions(Guid productId)
        {
            return _options.GetAll(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IProductOption GetOption(Guid productId, Guid id)
        {
            var option = _options.Get(id);
            if (option == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, JsonProductOption option)
        {
            _options.Create(productId, option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, JsonProductOption option)
        {
            _options.Update(id, option);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            _options.Delete(id);
        }

        public class JsonProduct : IProduct
        {
            public decimal DeliveryPrice { get; set; }

            public string Description { get; set; }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }
        }

        public class JsonProductOption : IProductOption
        {
            public string Description { get; set; }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public Guid ProductId { get; set; }
        }
    }
}
