using System;
using System.Collections.Generic;

namespace EvidencePractise_final1.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmploeeName { get; set; } = null!;

    public DateTime Joindate { get; set; }

    public string? Image { get; set; }

    public bool ActiveStatus { get; set; }

    public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
}
