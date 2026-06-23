using System;
using System.Collections.Generic;

namespace Работа_с_БД_из_кода.Models;

public partial class TrafficLog
{
    public Guid LogId { get; set; }

    public int StationId { get; set; }

    public DateTime LogDateTime { get; set; }

    public string TargetIp { get; set; } = null!;

    public string ApplicationName { get; set; } = null!;

    public long VolumeBytes { get; set; }

    public virtual Workstation Station { get; set; } = null!;
}
