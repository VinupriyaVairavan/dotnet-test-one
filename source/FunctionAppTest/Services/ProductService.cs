using AutoMapper;
using FunctionAppTest.Data;
using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Services;

public class ProductService(ProductCatalogueContext context, IMapper mapper) : IProductService
{
    public async Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = mapper.Map<Product>(request);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return mapper.Map<CreateProductResponse>(product);
    }
    
}