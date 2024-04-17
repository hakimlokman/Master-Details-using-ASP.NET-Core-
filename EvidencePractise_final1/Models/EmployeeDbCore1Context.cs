using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EvidencePractise_final1.Models;

public partial class EmployeeDbCore1Context : DbContext
{
    public EmployeeDbCore1Context()
    {
    }

    public EmployeeDbCore1Context(DbContextOptions<EmployeeDbCore1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=DESKTOP-2KRLH3S\\LOKMAN;Database=EmployeeDbCore1;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F1178068FAF");

            entity.ToTable("Employee");

            entity.Property(e => e.EmploeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.Joindate).HasColumnType("date");
        });

        modelBuilder.Entity<EmployeeSkill>(entity =>
        {
            entity.HasKey(e => e.EmployeeSkillId).HasName("PK__Employee__1CC7FE6C41841BC5");

            entity.ToTable("EmployeeSkill");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSkills)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__EmployeeS__Emplo__3B75D760");

            entity.HasOne(d => d.Skill).WithMany(p => p.EmployeeSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("FK__EmployeeS__Skill__3C69FB99");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skill__DFA091871688CBF4");

            entity.ToTable("Skill");

            entity.Property(e => e.SkillName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
