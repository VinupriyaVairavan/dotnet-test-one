using AutoMapper;
using FunctionAppTest.Data;
using FunctionAppTest.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionAppTest.Tests.ServiceTests
{
    public class ProductServiceTestsTest
    {
        private readonly ProductService _sut;
        private readonly Mock<GenericRepository<Product>> _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ServiceProvider _serviceProvider;
        private readonly Mock<ProductCatalogueContext> _productCatalogueContext;

        public ProductServiceTestsTest()
        {
            _productCatalogueContext = new Mock<ProductCatalogueContext>();
            _context = new Mock<GenericRepository<Product>>(_productCatalogueContext.Object);

            _mapperMock = new Mock<IMapper>();
            _sut = new ProductService(_context.Object, _mapperMock.Object);
        }
        
        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, ProductName = "Test Product" };
            _context.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _sut.GetProductAsync(productId);

            // Assert
            _context.Verify(x => x.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _context.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _sut.GetProductAsync(productId);

            // Assert
            _context.Verify(x => x.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_CreatesProductSuccessfully()
        {
            // Arrange
            var createProductRequest = new CreateProductRequest { ProductName = "New Product" };
            var product = new Product { Id = 1, ProductName = createProductRequest.ProductName };
            _mapperMock.Setup(x => x.Map<Product>(createProductRequest)).Returns(product);
            _context.Setup(x => x.AddAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateProductAsync(createProductRequest);

            // Assert
            _context.Verify(x => x.AddAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_DeletesProductSuccessfully()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, ProductName = "Test Product" };
            _context.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _context.Setup(x => x.DeleteAsync(productId)).Returns(Task.CompletedTask);

            // Act
            await _sut.RemoveProductAsync(productId);

            // Assert
            _context.Verify(x => x.DeleteAsync(productId), Times.Once);
        }        
    }
}