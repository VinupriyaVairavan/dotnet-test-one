using AutoMapper;
using FunctionAppTest.Data;
using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Services;

public class ProductService : IProductService
{
    private readonly ProductCatalogueContext _context;
    private readonly IMapper _mapper;

    public ProductService(ProductCatalogueContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = _mapper.Map<Product>(request);
        product.CreatedOn = DateTime.UtcNow;
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return _mapper.Map<CreateProductResponse>(product);
    }

    public async Task<GetProductResponse?> GetProductAsync(int productId)
    {
        Product? product = new Product();
        // try
        // {
            product = await _context.Products.FindAsync(productId);
            
            if (product == null)
                return null;
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }
        return _mapper.Map<GetProductResponse>(product);
    }

    public async Task<bool> RemoveProductAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if(product == null)
            return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UpdateProductResponse?> UpdateProductAsync(UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(request.Id);
        
        if(product == null)
            return null;

        _mapper.Map(request, product);
        product.ModifiedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateProductResponse>(product);
    }
}