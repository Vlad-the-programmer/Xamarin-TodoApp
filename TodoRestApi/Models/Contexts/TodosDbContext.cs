using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoRestApi.Models;

namespace TodoRestApi.Models.Contexts;

public partial class TodosDbContext : DbContext
{
    public TodosDbContext()
    {
    }

    public TodosDbContext(DbContextOptions<TodosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Todo> Todos { get; set; }

    public virtual DbSet<TodoTag> TodoTags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TodosDB;Integrated Security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC0716AABBA5");
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07C823F374");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Todos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notes__user_id__3A81B327");
        });

        modelBuilder.Entity<TodoTag>(entity =>
        {
            entity.HasKey(e => new { e.TodoId, e.TagId }).HasName("PK_NoteTag");

            entity.HasOne(d => d.Tag).WithMany(p => p.TodoTags).HasConstraintName("FK_NoteTags_Tags_TagId");

            entity.HasOne(d => d.Todo).WithMany(p => p.TodoTags).HasConstraintName("FK_NoteTags_Notes_NoteId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07F3042495");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
