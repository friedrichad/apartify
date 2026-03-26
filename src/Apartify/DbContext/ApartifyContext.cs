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

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Resident> Residents { get; set; }

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
            entity.HasKey(e => e.ApartmentId).HasName("PK__Apartmen__CBDF576454A47502");

            entity.ToTable("Apartment");

            entity.Property(e => e.Area).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Building).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Apartment__Build__4316F928");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Building__5463CDC41B9DFF03");

            entity.ToTable("Building");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D3469481AF22B");

            entity.ToTable("Contract");

            entity.HasOne(d => d.Apartment).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Contract__Apartm__44FF419A");

            entity.HasOne(d => d.Resident).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("FK__Contract__Reside__45F365D3");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__33A8517A3AAE731A");

            entity.ToTable("Request");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Apartment).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Request__Apartme__47DBAE45");

            entity.HasOne(d => d.Resident).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("FK__Request__Residen__46E78A0C");
        });

        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.ResidentId).HasName("PK__Resident__07FB00DCDD0B76C1");

            entity.ToTable("Resident");

            entity.HasIndex(e => e.UserId, "UQ__Resident__1788CC4DE1C6B400").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Resident)
                .HasForeignKey<Resident>(d => d.UserId)
                .HasConstraintName("FK__Resident__UserId__440B1D61");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CC4CB06BCA84");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Username, "UQ__UserAcco__536C85E46F5E8652").IsUnique();

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
