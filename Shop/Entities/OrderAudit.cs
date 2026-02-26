using System;
using System.Collections.Generic;

namespace Shop.Entities;

public partial class OrderAudit
{
    public int OrderAuditId { get; set; }

    public int OrderId { get; set; }

    public string Operations { get; set; } = null!;

    public string? OldStatus { get; set; }

    public string? NewStatis { get; set; }

    public DateTime ChangedAt { get; set; }

    public int? ChangedByUserId { get; set; }

    public virtual User? ChangedByUser { get; set; }

    public virtual Order Order { get; set; } = null!;
}
