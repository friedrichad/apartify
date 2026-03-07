using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Building
{
    public int BuildingId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
