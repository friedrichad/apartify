using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Status { get; set; }

    public virtual Resident? Resident { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
