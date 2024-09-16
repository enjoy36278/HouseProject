using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public partial class dbHouseContext : DbContext
{
    public dbHouseContext(DbContextOptions<dbHouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<House> House { get; set; }

    public virtual DbSet<HouseType> HouseType { get; set; }

    public virtual DbSet<Login> Login { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityID).HasName("PK__City__F2D21A9601C6ADBE");

            entity.Property(e => e.CityName).HasMaxLength(20);
        });

        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.HouseID).HasName("PK__House__085D12AF1A8A57DA");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.District).HasMaxLength(20);
            entity.Property(e => e.HouseName).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.House)
                .HasForeignKey(d => d.CityID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__House__CityID__3D5E1FD2");

            entity.HasOne(d => d.Member).WithMany(p => p.House)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__House__MemberID__3F466844");

            entity.HasOne(d => d.Type).WithMany(p => p.House)
                .HasForeignKey(d => d.TypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__House__TypeID__3E52440B");
        });

        modelBuilder.Entity<HouseType>(entity =>
        {
            entity.HasKey(e => e.TypeID).HasName("PK__HouseTyp__516F03950B1E5F67");

            entity.Property(e => e.TypeName).HasMaxLength(20);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Account);

            entity.Property(e => e.Account)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B3856C6CD0C");

            entity.Property(e => e.MemberName).HasMaxLength(30);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
