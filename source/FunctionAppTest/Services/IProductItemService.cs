using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Services;

public interface IProductItemService
{
    Task<GetProductItemResponse?> GetProductItemAsync(int productId);
    Task<List<GetProductItemResponse>?> GetProductItemsByProductIdAsync(int productId);
    Task<CreateProductItemResponse> CreateProductItemAsync(CreateProductItemRequest request);
    Task<bool> UpdateProductItemAsync(UpdateProductItemRequest request);
    Task<bool> RemoveProductItemAsync(int productId);
}