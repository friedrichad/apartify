using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class ServiceFee
{
    public int FeeId { get; set; }

    public int? ApartmentId { get; set; }

    public string? Month { get; set; }

    public decimal? Amount { get; set; }

    public bool? Paid { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
