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

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Resident> Residents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ServiceFee> ServiceFees { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

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
            entity.HasKey(e => e.ApartmentId).HasName("PK__Apartmen__CBDF5764EAFF41CA");

            entity.ToTable("Apartment");

            entity.Property(e => e.Area).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Building).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Apartment__Build__5070F446");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Building__5463CDC44008864B");

            entity.ToTable("Building");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D3469E5CF65FE");

            entity.ToTable("Contract");

            entity.HasOne(d => d.Apartment).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Contract__Apartm__5629CD9C");

            entity.HasOne(d => d.Resident).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("FK__Contract__Reside__571DF1D5");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__FC681851ED3ADBAF");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.MethodName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__33A8517A2B6550B7");

            entity.ToTable("Request");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Apartment).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Request__Apartme__6FE99F9F");

            entity.HasOne(d => d.Resident).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("FK__Request__Residen__6EF57B66");

            entity.HasOne(d => d.Staff).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Request__StaffId__70DDC3D8");
        });

        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.ResidentId).HasName("PK__Resident__07FB00DC6B37DDB6");

            entity.ToTable("Resident");

            entity.HasIndex(e => e.UserId, "UQ__Resident__1788CC4DAE91F27C").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Resident)
                .HasForeignKey<Resident>(d => d.UserId)
                .HasConstraintName("FK__Resident__UserId__534D60F1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AE7C378E8");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Users).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<UserAccount>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__UserId__52593CB8"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__RoleId__5165187F"),
                    j =>
                    {
                        j.HasKey("RoleId", "UserId").HasName("PK__UserRole__5B8242DE38859616");
                        j.ToTable("UserRole");
                    });
        });

        modelBuilder.Entity<ServiceFee>(entity =>
        {
            entity.HasKey(e => e.FeeId).HasName("PK__ServiceF__B387B229A64AAEBB");

            entity.ToTable("ServiceFee");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Month)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Paid).HasDefaultValue(false);

            entity.HasOne(d => d.Apartment).WithMany(p => p.ServiceFees)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__ServiceFe__Apart__6B24EA82");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB17E3D33132");

            entity.HasIndex(e => e.UserId, "UQ__Staff__1788CC4D8FD7F0CF").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Building).WithMany(p => p.Staff)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Staff__BuildingI__5441852A");

            entity.HasOne(d => d.User).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.UserId)
                .HasConstraintName("FK__Staff__UserId__5535A963");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B8D1BC69C");

            entity.ToTable("Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Fee).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FeeId)
                .HasConstraintName("FK__Transacti__FeeId__6C190EBB");

            entity.HasOne(d => d.Method).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.MethodId)
                .HasConstraintName("FK__Transacti__Metho__6E01572D");

            entity.HasOne(d => d.Resident).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("FK__Transacti__Resid__6D0D32F4");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CC4CFF1FB71D");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Username, "UQ__UserAcco__536C85E4220E4952").IsUnique();

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
