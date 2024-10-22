using AutoMapper;
using FunctionAppTest.Data;
using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;
using FunctionAppTest.Repository;

namespace FunctionAppTest.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Product> _productRepository;

    public ProductService(IGenericRepository<Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = _mapper.Map<Product>(request);
        product.CreatedOn = DateTime.UtcNow;
        await _productRepository.AddAsync(product);
        return _mapper.Map<CreateProductResponse>(product);
    }

    public async Task<GetProductResponse?> GetProductAsync(int productId)
    {
        Product? product = new Product();
        product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            return null;
        return _mapper.Map<GetProductResponse>(product);
    }

    public async Task<bool> RemoveProductAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if(product == null)
            return false;
        await _productRepository.DeleteAsync(productId);
        return true;
    }

    public async Task<UpdateProductResponse?> UpdateProductAsync(UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        
        if(product == null)
            return null;

        _mapper.Map(request, product);
        
        await _productRepository.UpdateAsync(product);
        return _mapper.Map<UpdateProductResponse>(product);
    }
}