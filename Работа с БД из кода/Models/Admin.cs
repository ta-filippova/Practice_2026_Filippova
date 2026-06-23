using System;
using System.Collections.Generic;

namespace Работа_с_БД_из_кода.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string FullName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Workstation> Workstations { get; set; } = new List<Workstation>();
}
