using System;
using System.Collections.Generic;

namespace Работа_с_БД_из_кода.Models;

public partial class Software
{
    public int SoftwareId { get; set; }

    public string SoftwareName { get; set; } = null!;

    public string? Developer { get; set; }

    public virtual ICollection<StationSoftware> StationSoftwares { get; set; } = new List<StationSoftware>();
}
