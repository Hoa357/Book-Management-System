using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHOM
{
    internal class KetNoi
    {

        SqlCommand cmd;
        private static string connectring =@"Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectring);
        }
       
    }
}
