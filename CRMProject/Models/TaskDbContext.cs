using System;
using System.Collections.Generic;
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

    public virtual DbSet<CustomerInventoryListing> CustomerInventoryListings { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Lead> Leads { get; set; }

    public virtual DbSet<MyProfile> MyProfile { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<TaskDashboard> TaskDashboards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07496A5F34");

            entity.HasIndex(e => e.Email, "unique_email").IsUnique();

            entity.HasIndex(e => e.Mobile, "unique_mobile").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Property).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("hot");
        });

        modelBuilder.Entity<CustomerInventoryListing>(entity =>
        {
            entity.HasKey(e => e.Ciid);

            entity.ToTable("CustomerInventoryListing");

            entity.Property(e => e.Ciid).HasColumnName("CIid");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
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
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.AskingPrice).HasColumnName("askingPrice");
            entity.Property(e => e.Date)
                 .HasMaxLength(50)
                 .HasColumnName("date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.InventoryId).HasColumnName("inventoryId");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Mobile).HasColumnName("mobile");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Property).HasColumnName("property");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Stage)
                .HasMaxLength(50)
                .HasColumnName("stage");
            entity.Property(e => e.TitleCheck)
                .HasMaxLength(50)
                .HasColumnName("titleCheck");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Inventory).WithMany(p => p.Leads)
                .HasForeignKey(d => d.InventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leads_inventory");
        });

        modelBuilder.Entity<MyProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MyProfil__3214EC071409C45D");

            entity.ToTable("MyProfile");

            entity.HasIndex(e => e.Email, "UQ__MyProfil__AB6E61640F4C771D").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.ProfileUrl)
                .HasMaxLength(50)
                .HasColumnName("profileUrl");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("refreshToken");
            entity.Property(e => e.TokenCreated)
                .HasColumnType("datetime")
                .HasColumnName("tokenCreated");
            entity.Property(e => e.TokenExpires)
                .HasColumnType("datetime")
                .HasColumnName("tokenExpires");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RefreshToken");

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TaskDashboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3213E83FBE026388");

            entity.ToTable("TaskDashboard");

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
            entity.Property(e => e.Task)
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
