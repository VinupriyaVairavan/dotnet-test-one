namespace FunctionAppTest.Models.Request;

public class CreateProductRequest
{
    public string ProductName { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}