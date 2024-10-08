﻿using System;
using System.Collections.Generic;
using Jacmazon_ECommerce.Models.LoginContext;
using Microsoft.EntityFrameworkCore;

namespace Jacmazon_ECommerce.Data;

public partial class LoginContext : DbContext
{
    public LoginContext(DbContextOptions<LoginContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Refresh_Tokens");

            entity.HasIndex(e => e.RefreshToken, "IX_Tokens").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiredDate).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(256)
                .HasComment("長期Token");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_db_user");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Approved).HasComment("啟用");
            entity.Property(e => e.CreateDate)
                .HasComment("新增日期")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasComment("電子信箱");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("名稱");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("密碼");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasComment("電話");
            entity.Property(e => e.Rank).HasComment("權限");
            entity.Property(e => e.Salt)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.UpdateDate)
                .HasComment("更新日期")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
