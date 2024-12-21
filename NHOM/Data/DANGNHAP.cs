namespace NHOM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DANGNHAP")]
    public partial class DANGNHAP
    {
        [Key]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public int? Loai { get; set; }

        public int MaNguoiDung { get; set; }

        [StringLength(255)]
        public string ResetMK { get; set; }

        public DateTime? TimeReset { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
    }
}
