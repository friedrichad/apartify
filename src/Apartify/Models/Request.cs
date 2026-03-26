using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Request
{
    public int RequestId { get; set; }

    public int? ResidentId { get; set; }

    public int? ApartmentId { get; set; }

    public string? Description { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Status { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual Resident? Resident { get; set; }
}
