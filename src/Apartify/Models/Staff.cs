using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Position { get; set; }

    public int? BuildingId { get; set; }

    public int? UserId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    public virtual UserAccount? User { get; set; }
}
