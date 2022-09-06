using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WPF_Demo.Models
{
    public partial class QLSachContext : DbContext
    {
        public QLSachContext()
        {
        }

        public QLSachContext(DbContextOptions<QLSachContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Sach> Saches { get; set; } = null!;
        public virtual DbSet<TacGium> TacGia { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-G0I3496\\SQLEXPRESS;Initial Catalog=QLSach;Integrated Security=True");
#pragma warning restore CS1030 // #warning directive
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.MaSach)
                    .HasName("PK__Sach__B235742DD0BCEDAE");

                entity.ToTable("Sach");

                entity.Property(e => e.MaSach)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.MaTg)
                    .HasMaxLength(10)
                    .HasColumnName("MaTG")
                    .IsFixedLength();

                entity.Property(e => e.TenSach).HasMaxLength(50);

                entity.HasOne(d => d.MaTgNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaTg)
                    .HasConstraintName("fk_S_TG");
            });

            modelBuilder.Entity<TacGium>(entity =>
            {
                entity.HasKey(e => e.MaTg)
                    .HasName("PK__TacGia__27250074D9AEF659");

                entity.Property(e => e.MaTg)
                    .HasMaxLength(10)
                    .HasColumnName("MaTG")
                    .IsFixedLength();

                entity.Property(e => e.TenTacGia).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
