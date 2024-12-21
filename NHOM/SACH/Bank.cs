using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHOM.KHACHHANG
{
  
    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string bin { get; set; }
        public string shortName { get; set; }
        public string logo { get; set; }
        public int transferSupported { get; set; }
        public int lookupSupported { get; set; }
        public string short_name { get; set; }
        public int support { get; set; }
        public int isTransfer { get; set; }
        public string swift_code { get; set; }
        public string customer_name
        {
            get
            {
                return $"({bin}) {shortName}";
            }
            set
            {
                // Setter không thực hiện gì, vì customer_name là thuộc tính chỉ đọc (read-only).
                // Bạn có thể xử lý setter nếu cần thiết, chẳng hạn như gán giá trị cho bin và shortName.

            }
        }

    }

   

    public class Bank
    {
        public string code { get; set; }
        public string desc { get; set; }
        public IList<Datum> data { get; set; }
    }


    
}
