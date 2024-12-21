namespace NHOM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADON")]
    public partial class HOADON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOADON()
        {
            CT_HoaDon = new HashSet<CT_HoaDon>();
        }

        [Key]
        public int SoHD { get; set; }

        public int? MaKH { get; set; }

        public DateTime? NgayHD { get; set; }

        public double? TongTien { get; set; }

        public double? SoTienTra { get; set; }

        public int MaNguoiNhap { get; set; }

        public int? MaGiam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_HoaDon> CT_HoaDon { get; set; }

        public virtual DISCOUNT DISCOUNT { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
    }
}
