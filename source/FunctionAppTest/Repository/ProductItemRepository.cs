using FunctionAppTest.Data;
using Microsoft.EntityFrameworkCore;

namespace FunctionAppTest.Repository;

public class ProductItemRepository(ProductCatalogueContext context)
    : GenericRepository<ProductItem>(context), IProductItemRepository
{
    public async Task<List<ProductItem>> GetProductItemsByProductIdAsync(int productId)
    {
        var product = context.Products.FindAsync(productId);
        
        return await context.ProductItems.Where(x => x.ProductId == productId).ToListAsync();
    }
}