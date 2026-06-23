using System;
using System.Collections.Generic;

namespace Работа_с_БД_из_кода.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string DomainLogin { get; set; } = null!;

    public virtual ICollection<Workstation> Workstations { get; set; } = new List<Workstation>();
}
