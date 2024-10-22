using System;
using System.Collections.Generic;

namespace FunctionAppTest.Data;

public partial class ProductItem
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual Product? Product { get; set; }
}
