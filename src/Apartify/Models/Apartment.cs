using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Apartment
{
    public int ApartmentId { get; set; }

    public int? BuildingId { get; set; }

    public string? Number { get; set; }

    public int? Floor { get; set; }

    public decimal? Area { get; set; }

    public virtual Building? Building { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
