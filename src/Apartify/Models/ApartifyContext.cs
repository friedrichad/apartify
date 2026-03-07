using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Apartify.Models;

public partial class ApartifyContext : DbContext
{
    public ApartifyContext()
    {
    }

    public ApartifyContext(DbContextOptions<ApartifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<Resident> Residents { get; set; }

    public virtual DbSet<ServiceFee> ServiceFees { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }
    private String GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionString"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasKey(e => e.ApartmentId).HasName("PK__Apartmen__CBDF5764E9DCD741");

            entity.ToTable("Apartment");

            entity.Property(e => e.Area).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Building).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Apartment__Build__47DBAE45");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Building__5463CDC4C1B2428C");

            entity.ToTable("Building");

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D3469FD7F72A9");

            entity.ToTable("Contract");

            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Apartment).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Contract__Apartm__4BAC3F29");

            entity.HasOne(d => d.Resident).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("FK__Contract__Reside__4CA06362");
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.MaintenanceId).HasName("PK__Maintena__E60542D55741DB1A");

            entity.ToTable("Maintenance");

            entity.Property(e => e.IssueDescription)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Apartment).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Maintenan__Apart__4E88ABD4");

            entity.HasOne(d => d.Staff).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Maintenan__Staff__4F7CD00D");
        });

        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.ResidentId).HasName("PK__Resident__07FB00DC0956D4C9");

            entity.ToTable("Resident");

            entity.HasIndex(e => e.UserId, "UQ__Resident__1788CC4DBB867C8D").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Resident)
                .HasForeignKey<Resident>(d => d.UserId)
                .HasConstraintName("FK__Resident__UserId__48CFD27E");
        });

        modelBuilder.Entity<ServiceFee>(entity =>
        {
            entity.HasKey(e => e.FeeId).HasName("PK__ServiceF__B387B2295482779D");

            entity.ToTable("ServiceFee");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Month)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.HasOne(d => d.Apartment).WithMany(p => p.ServiceFees)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__ServiceFe__Apart__4D94879B");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB17BC978E21");

            entity.HasIndex(e => e.UserId, "UQ__Staff__1788CC4DCC0DE638").IsUnique();

            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Building).WithMany(p => p.Staff)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Staff__BuildingI__49C3F6B7");

            entity.HasOne(d => d.User).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.UserId)
                .HasConstraintName("FK__Staff__UserId__4AB81AF0");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CC4C71363C9C");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Username, "UQ__UserAcco__536C85E48661F977").IsUnique();

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
