using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Services;

public interface IProductService
{
    Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request);
}