using System;
using System.Collections.Generic;
using CRMDashboardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Models;

public partial class TaskDbContext : DbContext
{
    public TaskDbContext()
    {
    }

    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Lead> Leads { get; set; }

    public virtual DbSet<TaskDashboard> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07496A5F34");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Property).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("hot");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventor__3213E83F431FF7AC");

            entity.ToTable("inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Bed)
                .HasMaxLength(10)
                .HasColumnName("bed");
            entity.Property(e => e.Floor)
                .HasMaxLength(10)
                .HasColumnName("floor");
            entity.Property(e => e.Lift).HasColumnName("lift");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.ParkFacing).HasColumnName("parkFacing");
            entity.Property(e => e.PlotSize)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("plotSize");
            entity.Property(e => e.PropertyStatus)
                .HasMaxLength(255)
                .HasColumnName("propertyStatus");
            entity.Property(e => e.PropertyType)
                .HasMaxLength(255)
                .HasColumnName("propertyType");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasColumnName("remarks");
            entity.Property(e => e.Rent)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("rent");
            entity.Property(e => e.StaffRoom).HasColumnName("staffRoom");
            entity.Property(e => e.StiltParking).HasColumnName("stiltParking");
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leads__3214EC0755FF6767");

            entity.Property(e => e.Area).HasMaxLength(50);
            entity.Property(e => e.AskingPrice).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Stage).HasMaxLength(50);
            entity.Property(e => e.TitleCheck).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3213E83FBE026388");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignTo)
                .HasMaxLength(255)
                .HasColumnName("assignTo");
            entity.Property(e => e.Date)
                .HasMaxLength(50)
                .HasColumnName("date");
            entity.Property(e => e.Labels)
                .HasMaxLength(255)
                .HasColumnName("labels");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Task1)
                .HasMaxLength(255)
                .HasColumnName("task");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
