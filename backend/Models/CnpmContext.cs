using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public partial class CnpmContext : DbContext
{
    public CnpmContext()
    {
    }

    public CnpmContext(DbContextOptions<CnpmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Hottel> Hottels { get; set; }
    public virtual DbSet<Tourist_Area> TouristAreas { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-1-ap-south-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.xdtzkeyxwcrhyqtbqifr;Password=r!*#&4U8_7#&drN;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Avt).HasColumnName("avt");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            entity.Property(e => e.ResetPasswordToken).HasColumnName("ResetPasswordToken");
            entity.Property(e => e.ResetPasswordExpiry)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ResetPasswordExpiry");

            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Role).HasColumnName("role");
        });
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");

            entity.HasKey(e => e.Id)
                  .HasName("refresh_tokens_pkey");

            entity.Property(e => e.Id)
                  .HasColumnName("id");

            entity.Property(e => e.UserId)
                  .HasColumnName("user_id")
                  .IsRequired();

            entity.Property(e => e.TokenHash)
                  .HasColumnName("token_hash")
                  .IsRequired();

            entity.Property(e => e.ExpiresAt)
                  .HasColumnName("expires_at")
                  .HasColumnType("timestamp without time zone");

            entity.Property(e => e.IsRevoked)
                  .HasColumnName("is_revoked");

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("now()")
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("created_at");

            entity.HasIndex(e => e.TokenHash)
                  .HasDatabaseName("ix_refresh_tokens_token_hash");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("fk_refresh_tokens_users");
        });
        modelBuilder.Entity<Hottel>(entity =>
        {
            entity.ToTable("hottels");

            entity.HasKey(e => e.Id)
                  .HasName("hottels_pkey");

            entity.Property(e => e.Id)
                  .HasColumnName("id");

            entity.Property(e => e.Name)
                  .HasColumnName("name");

            entity.Property(e => e.Img)
                  .HasColumnName("img");

            entity.Property(e => e.Address)
                  .HasColumnName("address");

            entity.Property(e => e.Description)
                  .HasColumnName("description");

            entity.Property(e => e.CreatedOn)
                  .HasDefaultValueSql("now()")
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("created_at");
        });
        modelBuilder.Entity<Tourist_Area>(entity =>
        {
            entity.ToTable("tourist_areas");

            entity.HasKey(e => e.Id)
                  .HasName("tourist_areas_pkey");

            entity.Property(e => e.Id)
                  .HasColumnName("id");

            entity.Property(e => e.Name)
                  .HasColumnName("name");

            entity.Property(e => e.Img)
                  .HasComment("img");

            entity.Property(e => e.Address)
                  .HasColumnName("address");

            entity.Property(e => e.Description)
                  .HasColumnName("description");

            entity.Property(e => e.CreatedOn)
                  .HasDefaultValueSql("now()")
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("created_at");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
