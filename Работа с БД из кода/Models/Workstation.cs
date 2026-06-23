using System;
using System.Collections.Generic;

namespace Работа_с_БД_из_кода.Models;

public partial class Workstation
{
    public int StationId { get; set; }

    public string NetworkName { get; set; } = null!;

    public string Ipaddress { get; set; } = null!;

    public string Macaddress { get; set; } = null!;

    public string? InstallationPlace { get; set; }

    public int? EmployeeId { get; set; }

    public int? AdminId { get; set; }

    public bool? IsActive { get; set; }

    public decimal PurchasePrice { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<StationSoftware> StationSoftwares { get; set; } = new List<StationSoftware>();

    public virtual ICollection<TrafficLog> TrafficLogs { get; set; } = new List<TrafficLog>();
}
