using FunctionAppTest.Models.Request;
using FunctionAppTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunctionAppTest.Triggers
{
    public class ProductTrigger
    {
        #region Constants
        private const string LOGINFO = "trigger function processed a request.";
        private const string NOT_FOUND = "Product not found.";
        private const string INVALID_REQUEST = "Invalid request data.";
        private const string REMOVED = "Product removed successfully.";
        private const string UPDATED = "Product updated successfully.";
        #endregion
        private readonly ILogger<ProductTrigger> _logger;
        private readonly IProductService _productService;

        public ProductTrigger(ILogger<ProductTrigger> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Function("GetProduct")]
        public async Task<HttpResponseData> GetProduct([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/{id}")] 
        HttpRequestData req, string id)
        {
            _logger.LogInformation($"{nameof(GetProduct)} {LOGINFO}");
            var prodId = Convert.ToInt32(id);
            var product = await _productService.GetProductAsync(prodId);
            
            HttpResponseData response = req.CreateResponse();

            if(product == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(NOT_FOUND);
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync(product);
            }
            return response;
        }

        [Function("AddProduct")]
        public async Task<HttpResponseData> AddProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product")] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(AddProduct)} {LOGINFO}");
            var product = await req.ReadFromJsonAsync<CreateProductRequest>();
            
            HttpResponseData response = req.CreateResponse();
            
            if(product == null)
            {
                response.StatusCode= HttpStatusCode.BadRequest;
                await response.WriteStringAsync(INVALID_REQUEST);
            }
            else
            {
                var createProductResponse = await _productService.CreateProductAsync(product);
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync(createProductResponse);
            }

            return response;
        }

        [Function("UpdateProduct")]
        public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "product")]
         HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(UpdateProduct)} {LOGINFO}");
            var product = await req.ReadFromJsonAsync<UpdateProductRequest>();

            var response = req.CreateResponse();

            if(product == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(INVALID_REQUEST);
            }
            else
            {
                var updatedProduct = await _productService.UpdateProductAsync(product);
            
                if (updatedProduct != null)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    await response.WriteStringAsync(UPDATED);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    await response.WriteStringAsync(NOT_FOUND);
                }
            }
            return response;
        }

        [Function("RemoveProduct")]
        public async Task<HttpResponseData> RemoveProduct([HttpTrigger(AuthorizationLevel.Anonymous, "delete", 
        Route = "product/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation($"{nameof(RemoveProduct)} trigger function processed a request.");
            
            var isRemoved = await _productService.RemoveProductAsync(Convert.ToInt32(id));

            var response = req.CreateResponse();
            if (isRemoved)
            {
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteStringAsync(REMOVED);
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                await response.WriteStringAsync(NOT_FOUND);
            }

            return response;
        }
    }
}
