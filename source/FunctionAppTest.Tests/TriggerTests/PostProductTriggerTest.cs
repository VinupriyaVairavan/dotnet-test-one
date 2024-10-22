namespace FunctionAppTest.Tests.Triggers;

public class PostProductTriggerTest
{
    private readonly Mock<ILogger<ProductTrigger>> _loggerMock;
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IProductItemService> _productItemServiceMock;
    private readonly ProductTrigger _sut;
    private readonly CreateProductRequest createProductRequest;

    public PostProductTriggerTest()
    {
        _loggerMock = new Mock<ILogger<ProductTrigger>>();
        _productServiceMock = new Mock<IProductService>();
        _productItemServiceMock = new Mock<IProductItemService>();
        _sut = new ProductTrigger(_loggerMock.Object, _productServiceMock.Object, _productItemServiceMock.Object);
        createProductRequest = new CreateProductRequest() { ProductName = "Product", CreatedBy = "Test user" };
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequest_WhenProductIsNull()
    {
        // Arrange
        MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody("null")
                .Build();

        // Act
        var response = await _sut.AddProduct(mockHttpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOk_WhenProductIsCreated()
    {
        // Arrange

        MockHttpRequestData mockHttpRequest =
            new MockHttpRequestDataBuilder()
                .WithDefaultJsonSerializer()
                .WithFakeFunctionContext()
                .WithRawJsonBody(JsonSerializer.Serialize(createProductRequest))
                .Build();
        _productServiceMock.Setup(x => x.CreateProductAsync(It.IsAny<CreateProductRequest>()))
            .ReturnsAsync(new CreateProductResponse()
            {
                ProductName = createProductRequest.ProductName
            });

        // Act
        var response = await _sut.AddProduct(mockHttpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}