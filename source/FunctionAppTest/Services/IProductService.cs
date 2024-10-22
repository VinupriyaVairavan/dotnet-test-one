using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Services;

public interface IProductService
{
    Task<GetProductResponse?> GetProductAsync(int productId);
    Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request);
    Task<UpdateProductResponse?> UpdateProductAsync(UpdateProductRequest request);
    Task<bool> RemoveProductAsync(int productId);
}