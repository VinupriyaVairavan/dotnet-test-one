using AutoMapper;
using FunctionAppTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionAppTest.Tests.ServiceTests
{
    public class ProductServiceTestsTest
    {
        private readonly ProductService _sut;
        private readonly ProductCatalogueContext _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ServiceProvider _serviceProvider;

        public ProductServiceTestsTest()
        {
            var services = new ServiceCollection();
            // var options = new DbContextOptionsBuilder<ProductCatalogueContext>()
            //     .UseInMemoryDatabase(databaseName: "testdb")
            //     .Options;
            services.AddDbContext<ProductCatalogueContext>(opt 
                => opt.UseInMemoryDatabase(databaseName: "testdb"));
            
            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetService<ProductCatalogueContext>();
            _mapperMock = new Mock<IMapper>();
            _sut = new ProductService(_context, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var createProductRequest = new CreateProductRequest { ProductName = "Test Product", CreatedBy = "Test User" };
            var product = new Product { ProductName = createProductRequest.ProductName, CreatedBy = createProductRequest.CreatedBy };
            _mapperMock.Setup(m => m.Map<Product>(It.IsAny<CreateProductRequest>())).Returns(product);
        
            // Act
            var result = await _sut.CreateProductAsync(createProductRequest);
        
            // Assert
            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == createProductRequest.ProductName);
            Assert.NotNull(productInDb);
            Assert.Equal(createProductRequest.ProductName, productInDb.ProductName);
        }
        
        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "Test Product", CreatedBy = "Test User" };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        
            // Act
            var result = await _sut.GetProductAsync(product.Id);
        
            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.ProductName, result.ProductName);
        }
        
        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var result = await _sut.GetProductAsync(999);
        
            // Assert
            Assert.Null(result);
        }
    }
}