using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;
using FunctionAppTest.Options;
using FunctionAppTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionAppTest.Triggers
{
    public class ProductTrigger
    {
        private readonly ILogger<ProductTrigger> _logger;
        private readonly IProductService _productService;

        public ProductTrigger(ILogger<ProductTrigger> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Function("GetProduct")]
        public IActionResult GetProduct([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GetProduct)} trigger function processed a request.");
            return new OkObjectResult("<Return Product>");
        }

        [Function("AddProduct")]
        public async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, 
            [FromBody] CreateProductRequest product)
        {
            _logger.LogInformation($"{nameof(AddProduct)} trigger function processed a request.");
            var response = await _productService.CreateProductAsync(product);
            return new OkObjectResult(response);
        }

        [Function("UpdateProduct")]
        public IActionResult UpdateProduct([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(UpdateProduct)} trigger function processed a request.");
            return new OkObjectResult("<Add Product>");
        }

        [Function("RemoveProduct")]
        public IActionResult RemoveProduct([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(RemoveProduct)} trigger function processed a request.");
            return new OkObjectResult("<Product Removed>");
        }
    }
}
