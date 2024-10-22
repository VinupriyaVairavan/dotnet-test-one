namespace FunctionAppTest.Tests.Triggers;

public class GetProductTriggerTest
{
    private readonly Mock<ILogger<ProductTrigger>> _loggerMock;
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IProductItemService> _productItemServiceMock;
    private readonly ProductTrigger _sut;
    private readonly GetProductRequest _getProductRequest;

    public GetProductTriggerTest()
    {
        _loggerMock = new Mock<ILogger<ProductTrigger>>();
        _productServiceMock = new Mock<IProductService>();
        _productItemServiceMock = new Mock<IProductItemService>();
        _sut = new ProductTrigger(_loggerMock.Object, _productServiceMock.Object, _productItemServiceMock.Object);
        _getProductRequest = new GetProductRequest() { Id = 100 };
    }

    [Fact]
    public async Task GetProduct_ReturnsBadRequest_WhenProductIsNotFound()
    {
        // Arrange

        MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody(JsonSerializer.Serialize(_getProductRequest))
                .Build();

        // Act
        var response = await _sut.GetProduct(mockHttpRequest, _getProductRequest.Id.ToString());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetProduct_ReturnsOk_WhenProductIsAvailable()
    {
        // Arrange
        MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody(JsonSerializer.Serialize(_getProductRequest))
                .Build();
        _productServiceMock.Setup(x => x.GetProductAsync(It.IsAny<int>()))
            .ReturnsAsync(new GetProductResponse()
            {
                ProductName = "Product name",
                Id = _getProductRequest.Id,
                CreatedBy = "Test User"
            });

        // Act
        var response = await _sut.GetProduct(mockHttpRequest, _getProductRequest.Id.ToString());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}