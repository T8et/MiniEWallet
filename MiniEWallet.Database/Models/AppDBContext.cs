using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniEWallet.Database.Models;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblTranType> TblTranTypes { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=MiniEWallet;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity.HasKey(e => e.AccId);

            entity.ToTable("Tbl_Account");

            entity.Property(e => e.AccId).HasColumnName("Acc_Id");
            entity.Property(e => e.AccActive).HasColumnName("Acc_Active");
            entity.Property(e => e.AccAddress)
                .HasMaxLength(200)
                .HasColumnName("Acc_Address");
            entity.Property(e => e.AccBalance).HasColumnName("Acc_Balance");
            entity.Property(e => e.AccName)
                .HasMaxLength(50)
                .HasColumnName("Acc_Name");
            entity.Property(e => e.AccPassword).HasColumnName("Acc_Password");
            entity.Property(e => e.AccPhone)
                .HasMaxLength(13)
                .HasColumnName("Acc_Phone");
            entity.Property(e => e.TimeLog)
                .HasColumnType("datetime")
                .HasColumnName("Time_Log");
        });

        modelBuilder.Entity<TblTranType>(entity =>
        {
            entity.HasKey(e => e.TranType);

            entity.ToTable("Tbl_TranType");

            entity.Property(e => e.TranType).HasColumnName("Tran_Type");
            entity.Property(e => e.TimeLog)
                .HasColumnType("datetime")
                .HasColumnName("Time_Log");
            entity.Property(e => e.TranDescription)
                .HasMaxLength(50)
                .HasColumnName("Tran_Description");
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.TranId);

            entity.ToTable("Tbl_Transaction");

            entity.Property(e => e.TranId).HasColumnName("Tran_Id");
            entity.Property(e => e.FrAccId).HasColumnName("FrAcc_Id");
            entity.Property(e => e.TimeLog)
                .HasColumnType("datetime")
                .HasColumnName("Time_Log");
            entity.Property(e => e.ToAccId).HasColumnName("ToAcc_Id");
            entity.Property(e => e.TranType).HasColumnName("Tran_Type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
