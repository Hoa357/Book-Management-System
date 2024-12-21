namespace NHOM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHAPSACH")]
    public partial class NHAPSACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHAPSACH()
        {
            CT_NHAPSACH = new HashSet<CT_NHAPSACH>();
        }

        [Key]
        public int MaNS { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayNS { get; set; }

        public double? TongTien { get; set; }

        public int MaNguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_NHAPSACH> CT_NHAPSACH { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
    }
}
