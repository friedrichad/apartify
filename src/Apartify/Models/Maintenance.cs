using System;
using System.Collections.Generic;

namespace Apartify.Models;

public partial class Maintenance
{
    public int MaintenanceId { get; set; }

    public int? ApartmentId { get; set; }

    public int? StaffId { get; set; }

    public string? IssueDescription { get; set; }

    public DateOnly? ReportDate { get; set; }

    public string? Status { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual Staff? Staff { get; set; }
}
