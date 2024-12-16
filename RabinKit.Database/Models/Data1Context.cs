using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RabinKit.Database.Models;

public partial class Data1Context : DbContext
{
    public Data1Context()
    {
    }

    public Data1Context(DbContextOptions<Data1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<PerformanceTest> PerformanceTests { get; set; }

    public virtual DbSet<PerformanceTestAttempt> PerformanceTestAttempts { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<TaskAttempt> TaskAttempts { get; set; }

    public virtual DbSet<TaskComponent> TaskComponents { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TaskTestAttemptRelation> TaskTestAttemptRelations { get; set; }

    public virtual DbSet<TestValue> TestValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=F:\\Diplom\\data1.db3");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerformanceTest>(entity =>
        {
            entity.ToTable("performance_tests");

            entity.HasIndex(e => e.TaskDescriptionId, "ix_performance_tests_task_description_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PrepareScript).HasColumnName("prepare_script");
            entity.Property(e => e.TaskDescriptionId).HasColumnName("task_description_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.TaskDescription).WithMany(p => p.PerformanceTests).HasForeignKey(d => d.TaskDescriptionId);
        });

        modelBuilder.Entity<PerformanceTestAttempt>(entity =>
        {
            entity.ToTable("performance_test_attempts");

            entity.HasIndex(e => e.PerformanceTestId, "ix_performance_test_attempts_performance_test_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PerformanceTestId).HasColumnName("performance_test_id");
            entity.Property(e => e.Runs).HasColumnName("runs");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.PerformanceTest).WithMany(p => p.PerformanceTestAttempts).HasForeignKey(d => d.PerformanceTestId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("students");

            entity.HasIndex(e => e.Id, "IX_students_Id").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasColumnType("INTEGER")
                .HasColumnName("created_at");
            entity.Property(e => e.Group).HasColumnName("group");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("INTEGER")
                .HasColumnName("updated_at");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<TaskAttempt>(entity =>
        {
            entity.ToTable("task_attempts");

            entity.HasIndex(e => e.Id, "IX_task_attempts_id").IsUnique();

            entity.HasIndex(e => e.TaskId, "ix_task_attempts_task_components_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Inputs).HasColumnName("inputs");
            entity.Property(e => e.IsPassed).HasColumnName("is_passed");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAttempts).HasForeignKey(d => d.TaskId);
        });

        modelBuilder.Entity<TaskComponent>(entity =>
        {
            entity.ToTable("task_components");

            entity.HasIndex(e => e.Id, "IX_task_components_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("INTEGER")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Input).HasColumnName("input");
            entity.Property(e => e.IsTest)
                .HasDefaultValueSql("0")
                .HasColumnName("is_test");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Output)
                .HasColumnType("NUMERIC")
                .HasColumnName("output");
            entity.Property(e => e.Playground)
                .HasColumnType("NUMERIC")
                .HasColumnName("playground");
            entity.Property(e => e.Toolbox).HasColumnName("toolbox");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.ToTable("task_status");

            entity.HasIndex(e => e.Id, "IX_task_status_id").IsUnique();

            entity.HasIndex(e => e.TaskId, "IX_task_status_task_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("INTEGER")
                .HasColumnName("created_at");
            entity.Property(e => e.IsPassed).HasColumnName("is_passed");
            entity.Property(e => e.SolutionTime)
                .HasColumnType("INTEGER")
                .HasColumnName("solution_time");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("INTEGER")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<TaskTestAttemptRelation>(entity =>
        {
            entity.HasKey(e => new { e.AttemptId, e.TestId });

            entity.ToTable("task_test_attempt_relations");

            entity.HasIndex(e => e.TestId, "ix_task_test_attempt_relations_test_id");

            entity.Property(e => e.AttemptId).HasColumnName("attempt_id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Result).HasColumnName("result");

            entity.HasOne(d => d.Attempt).WithMany(p => p.TaskTestAttemptRelations).HasForeignKey(d => d.AttemptId);

            entity.HasOne(d => d.Test).WithMany(p => p.TaskTestAttemptRelations).HasForeignKey(d => d.TestId);
        });

        modelBuilder.Entity<TestValue>(entity =>
        {
            entity.ToTable("test_values");

            entity.HasIndex(e => e.TaskId, "ix_test_values_task_components_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.InputVars).HasColumnName("input_vars");
            entity.Property(e => e.OutputVars).HasColumnName("output_vars");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Task).WithMany(p => p.TestValues).HasForeignKey(d => d.TaskId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
