using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class PaymentMethod
{
    public int MethodId { get; set; }

    public string? MethodName { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
