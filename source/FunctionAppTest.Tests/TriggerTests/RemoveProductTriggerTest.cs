namespace FunctionAppTest.Tests.Triggers;

public class RemoveProductTriggerTest
{
    private readonly Mock<ILogger<ProductTrigger>> _loggerMock;
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductTrigger _sut;

    public RemoveProductTriggerTest()
    {
        _loggerMock = new Mock<ILogger<ProductTrigger>>();
        _productServiceMock = new Mock<IProductService>();
        _sut = new ProductTrigger(_loggerMock.Object, _productServiceMock.Object);
    }

    [Fact]
    public async Task RemoveProduct_ReturnsBadRequest_WhenProductIsNotFound()
    {
        // Arrange

        MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody(JsonSerializer.Serialize("100"))
                .Build();

        // Act
        var response = await _sut.RemoveProduct(mockHttpRequest, "100");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveProduct_ReturnsOk_WhenProductIsAvailable()
    {
        // Arrange
        MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody(JsonSerializer.Serialize("1"))
                .Build();
        _productServiceMock.Setup(x => x.RemoveProductAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var response = await _sut.RemoveProduct(mockHttpRequest, "1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}