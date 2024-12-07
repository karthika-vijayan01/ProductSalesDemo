using System;
using System.Collections.Generic;

namespace ProductSalesDemo.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public string? Qualification { get; set; }

    public string? ManagerName { get; set; }

    public int? Age { get; set; }

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
