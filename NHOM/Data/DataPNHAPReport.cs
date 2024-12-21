using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHOM.Data
{
    internal class DataPNHAPReport
    {
        public int MaSach { get; set; }

        public int? SoLuongNhap { get; set; }

        public double? DonGiaNhap { get; set; }

        public int MaNS { get; set; }

        
        public DateTime NgayNS { get; set; }

        public double TongTien { get; set; }

        public int MaNguoiDung { get; set; }


    }
}
