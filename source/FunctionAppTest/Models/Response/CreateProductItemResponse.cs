namespace FunctionAppTest.Models.Response;

public class CreateProductItemResponse
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }
}