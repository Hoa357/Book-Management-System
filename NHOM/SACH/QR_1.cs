using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHOM.KHACHHANG
{
    internal class QR_1
    {

        public class APIRequest
        {
            public int acqId { get; set; }
            public long accountNo { get; set; }
            public string accountName { get; set; }
            public int amount { get; set; }
            public string format { get; set; }
            public string template { get; set; }
        }

        public class ApiResponse
        {
            public ApiData data { get; set; }
        }

        public class ApiData
        {
            public string qrDataURL { get; set; }
        }

        // Định nghĩa Bank nếu cần thiết
        public class Bank
        {
            public List<BankData> data { get; set; }
        }

        public class BankData
        {
            public string custom_name { get; set; }
            public string bin { get; set; }
        }
    }
}
