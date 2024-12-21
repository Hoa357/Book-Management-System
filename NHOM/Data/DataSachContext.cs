using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NHOM.Data
{
    public partial class DataSachContext : DbContext
    {
        public DataSachContext()
            : base("name=DataSachContext")
        {
        }

        public virtual DbSet<CT_HoaDon> CT_HoaDon { get; set; }
        public virtual DbSet<CT_NHAPSACH> CT_NHAPSACH { get; set; }
        public virtual DbSet<DANGNHAP> DANGNHAP { get; set; }
        public virtual DbSet<DISCOUNT> DISCOUNT { get; set; }
        public virtual DbSet<HOADON> HOADON { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANG { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNG { get; set; }
        public virtual DbSet<NHAPSACH> NHAPSACH { get; set; }
        public virtual DbSet<SACH> SACH { get; set; }
        public virtual DbSet<THELOAI> THELOAI { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CT_HoaDon>()
                .Property(e => e.DonGia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CT_HoaDon>()
                .Property(e => e.ThanhTien)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DANGNHAP>()
                .Property(e => e.ResetMK)
                .IsUnicode(false);

            modelBuilder.Entity<DISCOUNT>()
                .Property(e => e.PhanTram)
                .HasPrecision(5, 2);

            modelBuilder.Entity<HOADON>()
                .HasMany(e => e.CT_HoaDon)
                .WithRequired(e => e.HOADON)
                .HasForeignKey(e => e.MaHoaDon);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.DANGNHAP)
                .WithRequired(e => e.NGUOIDUNG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.HOADON)
                .WithRequired(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.MaNguoiNhap);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.NHAPSACH)
                .WithRequired(e => e.NGUOIDUNG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NHAPSACH>()
                .HasMany(e => e.CT_NHAPSACH)
                .WithRequired(e => e.NHAPSACH)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SACH>()
                .Property(e => e.MaTheLoai)
                .IsFixedLength();

            modelBuilder.Entity<SACH>()
                .Property(e => e.Images)
                .IsFixedLength();

            modelBuilder.Entity<SACH>()
                .HasMany(e => e.CT_NHAPSACH)
                .WithRequired(e => e.SACH)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<THELOAI>()
                .Property(e => e.MaTheLoai)
                .IsFixedLength();
        }
    }
}
