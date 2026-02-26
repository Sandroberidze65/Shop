using System;
using System.Collections.Generic;

namespace Shop.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string? OrderNo { get; set; }

    public string Status { get; set; } = null!;

    public DateTime PlacedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public decimal ToTalAmount { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderAudit> OrderAudits { get; set; } = new List<OrderAudit>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
