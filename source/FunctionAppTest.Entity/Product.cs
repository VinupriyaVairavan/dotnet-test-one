using System;
using System.Collections.Generic;

namespace FunctionAppTest.Data;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
