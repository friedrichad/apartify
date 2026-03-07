using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Resident
{
    public int ResidentId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual UserAccount? User { get; set; }
}
