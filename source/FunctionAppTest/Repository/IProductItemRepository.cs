using FunctionAppTest.Data;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Repository;

public interface IProductItemRepository : IGenericRepository<ProductItem>
{
    Task<List<ProductItem>> GetProductItemsByProductIdAsync(int productId);
}