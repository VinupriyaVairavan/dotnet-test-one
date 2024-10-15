using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppTest.Triggers
{
    public class ProductTrigger
    {
        private readonly ILogger<ProductTrigger> _logger;
        public ProductTrigger(ILogger<ProductTrigger> logger)
        {
            _logger = logger;
        }

        [Function("GetProduct")]
        public IActionResult GetProduct([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GetProduct)} trigger function processed a request.");
            return new OkObjectResult("<Return Product>");
        }

        [Function("AddProduct")]
        public IActionResult AddProduct([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GetProduct)} trigger function processed a request.");
            return new OkObjectResult("<Add Product>");
        }

        [Function("UpdateProduct")]
        public IActionResult UpdateProduct([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GetProduct)} trigger function processed a request.");
            return new OkObjectResult("<Add Product>");
        }

        [Function("RemoveProduct")]
        public IActionResult RemoveProduct([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GetProduct)} trigger function processed a request.");
            return new OkObjectResult("<Product Removed>");
        }
    }
}
