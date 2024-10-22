namespace FunctionAppTest.Tests.Triggers
{
    
    public class UpdateProductTriggerTest
    {
        private readonly Mock<ILogger<ProductTrigger>> _loggerMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IProductItemService> _productItemServiceMock;
        private readonly ProductTrigger _sut;
        private readonly UpdateProductRequest _updateProductRequest;

        public UpdateProductTriggerTest()
        {
            _loggerMock = new Mock<ILogger<ProductTrigger>>();
            _productServiceMock = new Mock<IProductService>();
            _productItemServiceMock = new Mock<IProductItemService>();
            _sut = new ProductTrigger(_loggerMock.Object, _productServiceMock.Object, _productItemServiceMock.Object);
            _updateProductRequest = new UpdateProductRequest { Id = 1, ProductName = "Updated Product" };
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenProductIsNull()
        {
            // Arrange
            MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody("null")
                .Build();
            
            // Act
            var response = await _sut.UpdateProduct(mockHttpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk_WhenProductIsUpdated()
        {
            // Arrange
            MockHttpRequestData mockHttpRequest =
                new MockHttpRequestDataBuilder()
                    .WithDefaultJsonSerializer()
                    .WithFakeFunctionContext()
                    .WithRawJsonBody(JsonSerializer.Serialize(_updateProductRequest))
                    .Build();
            _productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<UpdateProductRequest>()))
                .ReturnsAsync(new UpdateProductResponse { 
                    ProductName = _updateProductRequest.ProductName, 
                    Id = _updateProductRequest.Id, 
                    CreatedBy = "Test User" });
            
            // Act
            var response = await _sut.UpdateProduct(mockHttpRequest);
        
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductIsNotUpdated()
        {
            // Arrange
            MockHttpRequestData mockHttpRequest =
                new MockHttpRequestDataBuilder()
                    .WithDefaultJsonSerializer()
                    .WithFakeFunctionContext()
                    .WithRawJsonBody(JsonSerializer.Serialize(_updateProductRequest))
                    .Build();

            // Act
            var response = await _sut.UpdateProduct(mockHttpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}