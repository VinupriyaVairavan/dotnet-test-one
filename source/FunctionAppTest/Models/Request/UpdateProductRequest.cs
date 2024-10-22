namespace FunctionAppTest.Models.Request;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ModifiedBy { get; set; }
}