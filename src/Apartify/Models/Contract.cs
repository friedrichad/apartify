using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? ApartmentId { get; set; }

    public int? ResidentId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual Resident? Resident { get; set; }
}
