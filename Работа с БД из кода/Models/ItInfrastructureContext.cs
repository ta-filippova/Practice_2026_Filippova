using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Работа_с_БД_из_кода.Models;

public partial class ItInfrastructureContext : DbContext
{
    public ItInfrastructureContext()
    {
    }

    public ItInfrastructureContext(DbContextOptions<ItInfrastructureContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Software> Softwares { get; set; }

    public virtual DbSet<StationSoftware> StationSoftwares { get; set; }

    public virtual DbSet<TrafficLog> TrafficLogs { get; set; }

    public virtual DbSet<Workstation> Workstations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=mirok\\SQLEXPRESS;Database=IT_Infrastructure;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E85BE79305");

            entity.HasIndex(e => e.Login, "UQ__Admins__5E55825BF00B7D2E").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1766122EB");

            entity.HasIndex(e => e.DomainLogin, "UQ__Employee__F857BF753E6E4478").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.DomainLogin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        modelBuilder.Entity<Software>(entity =>
        {
            entity.HasKey(e => e.SoftwareId).HasName("PK__Software__25EDB8DC76555381");

            entity.ToTable("Software");

            entity.Property(e => e.SoftwareId).HasColumnName("SoftwareID");
            entity.Property(e => e.Developer).HasMaxLength(100);
            entity.Property(e => e.SoftwareName).HasMaxLength(150);
        });

        modelBuilder.Entity<StationSoftware>(entity =>
        {
            entity.HasKey(e => new { e.StationId, e.SoftwareId }).HasName("PK__StationS__32867D50EA2B8B37");

            entity.ToTable("StationSoftware");

            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.SoftwareId).HasColumnName("SoftwareID");
            entity.Property(e => e.InstallDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Software).WithMany(p => p.StationSoftwares)
                .HasForeignKey(d => d.SoftwareId)
                .HasConstraintName("FK_SS_Software");

            entity.HasOne(d => d.Station).WithMany(p => p.StationSoftwares)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_SS_Station");
        });

        modelBuilder.Entity<TrafficLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__TrafficL__5E5499A865A56F9F");

            entity.Property(e => e.LogId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("LogID");
            entity.Property(e => e.ApplicationName).HasMaxLength(100);
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.TargetIp)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("TargetIP");

            entity.HasOne(d => d.Station).WithMany(p => p.TrafficLogs)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_Log_Station");
        });

        modelBuilder.Entity<Workstation>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("PK__Workstat__E0D8A6DDCEB5CF73");

            entity.HasIndex(e => e.Macaddress, "UQ__Workstat__95BFB0499B2FAE9E").IsUnique();

            entity.HasIndex(e => e.NetworkName, "UQ__Workstat__CC81C610050212FF").IsUnique();

            entity.HasIndex(e => e.Ipaddress, "UQ__Workstat__F0C25BE0403BB87E").IsUnique();

            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.InstallationPlace).HasMaxLength(100);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Macaddress)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("MACAddress");
            entity.Property(e => e.NetworkName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Admin).WithMany(p => p.Workstations)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_Station_Admin");

            entity.HasOne(d => d.Employee).WithMany(p => p.Workstations)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Station_Employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
