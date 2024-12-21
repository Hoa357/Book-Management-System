namespace NHOM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SACH")]
    public partial class SACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SACH()
        {
            CT_HoaDon = new HashSet<CT_HoaDon>();
            CT_NHAPSACH = new HashSet<CT_NHAPSACH>();
        }

        [Key]
        public int MaSach { get; set; }

        [StringLength(30)]
        public string MaTheLoai { get; set; }

        [StringLength(255)]
        public string TenSach { get; set; }

        [StringLength(255)]
        public string NhaXuatBan { get; set; }

        public int? NamXuatBan { get; set; }

        public int? SoLuongTon { get; set; }

        public double? DonGiaBan { get; set; }

        [StringLength(500)]
        public string Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_HoaDon> CT_HoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_NHAPSACH> CT_NHAPSACH { get; set; }

        public virtual THELOAI THELOAI { get; set; }
    }
}
