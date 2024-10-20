namespace FunctionAppTest.Models.Response;

public class CreateProductResponse
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}