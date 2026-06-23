using System;
using System.Collections.Generic;

namespace Работа_с_БД_из_кода.Models;

public partial class StationSoftware
{
    public int StationId { get; set; }

    public int SoftwareId { get; set; }

    public DateOnly? InstallDate { get; set; }

    public virtual Software Software { get; set; } = null!;

    public virtual Workstation Station { get; set; } = null!;
}
