using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public int? Status { get; set; }

    public virtual Resident? Resident { get; set; }
}
