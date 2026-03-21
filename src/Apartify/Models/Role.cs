using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<UserAccount> Users { get; set; } = new List<UserAccount>();
}
