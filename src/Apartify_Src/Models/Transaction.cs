using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? FeeId { get; set; }

    public int? ResidentId { get; set; }

    public int? MethodId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Note { get; set; }

    public virtual ServiceFee? Fee { get; set; }

    public virtual PaymentMethod? Method { get; set; }

    public virtual Resident? Resident { get; set; }
}
