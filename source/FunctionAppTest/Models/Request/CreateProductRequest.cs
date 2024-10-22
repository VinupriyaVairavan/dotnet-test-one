namespace FunctionAppTest.Models.Request;

public class CreateProductRequest
{
    public string ProductName { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
}