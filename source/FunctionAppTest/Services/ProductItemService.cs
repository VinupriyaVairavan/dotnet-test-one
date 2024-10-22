using AutoMapper;
using FunctionAppTest.Data;
using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;
using FunctionAppTest.Repository;

namespace FunctionAppTest.Services;

public class ProductItemService(IProductItemRepository productItemRepository, IMapper mapper)
    : IProductItemService
{
    public async Task<GetProductItemResponse?> GetProductItemAsync(int productItemId)
    {
        var productItem = await productItemRepository.GetByIdAsync(productItemId);

        return productItem == null ? null : mapper.Map<GetProductItemResponse>(productItem);
    }

    public async Task<List<GetProductItemResponse>?> GetProductItemsByProductIdAsync(int productId)
    {
        var productItem = await productItemRepository.GetByIdAsync(productId);

        return productItem == null ? null : mapper.Map<List<GetProductItemResponse>>(productItem);
    }
    
    public async Task<CreateProductItemResponse> CreateProductItemAsync(CreateProductItemRequest request)
    {
        var productItem = mapper.Map<ProductItem>(request);
        productItem.CreatedDate = DateTime.UtcNow;
        await productItemRepository.AddAsync(productItem);
        return mapper.Map<CreateProductItemResponse>(productItem);
    }

    public async Task<bool> UpdateProductItemAsync(UpdateProductItemRequest request)
    {
        var productItem = await productItemRepository.GetByIdAsync(request.Id);
        
        if(productItem == null)
            return false;

        mapper.Map(request, productItem);
        
        await productItemRepository.UpdateAsync(productItem);
        return true;
    }

    public async Task<bool> RemoveProductItemAsync(int productItemId)
    {
        var productItem = await productItemRepository.GetByIdAsync(productItemId);
        
        if(productItem == null)
            return false;
        
        await productItemRepository.DeleteAsync(productItemId);
        return true;
    }
}