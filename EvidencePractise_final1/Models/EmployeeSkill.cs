using System;
using System.Collections.Generic;

namespace EvidencePractise_final1.Models;

public partial class EmployeeSkill
{
    public int EmployeeSkillId { get; set; }

    public int? EmployeeId { get; set; }

    public int? SkillId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Skill? Skill { get; set; }
}
