using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NHOM.User
{
    public partial class UC_Accountcs : UserControl
    {
        private readonly string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";
        private int selectedRowIndex = -1;
        private int loai;
       
        private string ten; 

       
        public UC_Accountcs(string ten)
        {
            InitializeComponent();
            this.ten = ten;
            LoadDataToGrid();
        }

        private void UC_Accountcs_Load(object sender, EventArgs e)
        {
            txtTK.Enabled = false;
            txtMK.Enabled = false;
            comboBoxLoai.Enabled = false;
        
           

           
            string userName = ten;
            loai = int.Parse(GetUserLoai(userName)); 

           
           
            if (loai == 1)
            {
                // Nếu loai = 1, ẩn tất cả các tab
                tabControl1.TabPages.Clear();
            }
            else if (loai == 2)
            {
                // Nếu loai = 2, thêm hoặc thay đổi tabPage2
                tabControl1.TabPages.Add(tabPage2);

            }

            // Populate comboBoxLoai with values 1-5
            comboBoxLoai.Items.Add("1");
            comboBoxLoai.Items.Add("2");
            comboBoxLoai.Items.Add("3");
            comboBoxLoai.Items.Add("4");
            comboBoxLoai.Items.Add("5");
        }


        private string GetUserLoai(string userName)
        {
            string loai = string.Empty;
            string query = "SELECT Loai FROM DANGNHAP WHERE UserName = @UserName";
       
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Trim userName to avoid any trailing spaces
                        cmd.Parameters.AddWithValue("@UserName", userName.Trim());

                        object result = cmd.ExecuteScalar();

                        // Check if result is null
                        if (result != null)
                        {
                            loai = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy loại người dùng cho tên đăng nhập này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy loại người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return loai;
        }

        private void LoadDataToGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    string query = "SELECT Loai, UserName, Password FROM DANGNHAP";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    // Sau khi dữ liệu đã được tải vào DataGridView, bạn có thể kiểm tra các tên cột như sau:
                    // Duyệt qua các cột của DataGridView để kiểm tra tên cột
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        Console.WriteLine("Column Name: " + column.Name);  // In ra tên các cột trong DataGridView
                    }
                }
                catch (SqlException ex)
                {
                    string errorDetails = $"Lỗi kết nối cơ sở dữ liệu: {ex.Message} (Error Code: {ex.ErrorCode})";
                    MessageBox.Show(errorDetails, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            txtTK.Enabled = true;
            txtMK.Enabled = true;
            comboBoxLoai.Enabled = true ;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = dataGridView1.SelectedRows[0].Cells["UserName"].Value.ToString();
            string query = "DELETE FROM DANGNHAP WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Dữ liệu đã được xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtTK.Enabled = false;
            txtMK.Enabled = false;
            comboBoxLoai.Enabled = true;
         

                string userName = txtTK.Text.Trim();
                string passWord = txtMK.Text.ToString();
            int loai = int.Parse(comboBoxLoai.SelectedItem.ToString());

                string query = "UPDATE DANGNHAP SET  Loai = @Loai WHERE UserName = @UserName";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                         
                            cmd.Parameters.AddWithValue("@Loai", loai);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thông tin đã được cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataToGrid();
                            }
                            else
                            {
                                MessageBox.Show("Lỗi cập nhật dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
          
        }

        private bool CheckUserNameExists(string userName)
        {
            string query = "SELECT COUNT(*) FROM DANGNHAP WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kiểm tra tên tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtTK.Enabled = true;
            txtMK.Enabled = true;
            comboBoxLoai.Enabled = true;

            // Ensure that an item is selected in comboBoxLoai
            if (comboBoxLoai.SelectedItem == null || txtTK.Text == string.Empty || txtMK.Text == string.Empty)
            {
                    MessageBox.Show("Vui lòng tất cả các trường trên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Safely parse the selected item as an integer
                int loai;
                if (!int.TryParse(comboBoxLoai.SelectedItem.ToString(), out loai))
                {
                    MessageBox.Show("Giá trị trong comboBoxLoai không hợp lệ. Vui lòng chọn một số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userName = txtTK.Text.Trim();
                string passWord = txtMK.Text.Trim();

            // Check if the userName already exists
            if (CheckUserNameExists(userName))
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại. Vui lòng chọn tên tài khoản khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert the new user account into the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "INSERT INTO DANGNHAP (Loai, UserName, Passwd) VALUES (@Loai, @UserName, @Passwd)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Loai", loai);
                            cmd.Parameters.AddWithValue("@UserName", userName);
                            cmd.Parameters.AddWithValue("@Passwd", passWord);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Dữ liệu đã được thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataToGrid();  // Reload the grid with the updated data
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

                // Kiểm tra nếu không phải là nhấn vào tiêu đề cột (rowIndex == -1)
                if (e.RowIndex >= 0)
                {
                    // Lấy dữ liệu của dòng được chọn
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    // Cập nhật giá trị cho TextBox từ các cột trong DataGridView
                    txtTK.Text = selectedRow.Cells["UserName"].Value?.ToString() ?? string.Empty;
                    txtMK.Text = selectedRow.Cells["Password"].Value?.ToString() ?? string.Empty;

                    // Kiểm tra giá trị của cột "Loai" và cập nhật ComboBox nếu có
                    if (selectedRow.Cells["Loai"].Value != null)
                    {
                        var value = selectedRow.Cells["Loai"].Value.ToString();

                        // Kiểm tra xem giá trị có trong ComboBox không
                        if (comboBoxLoai.Items.Contains(value))
                        {
                            comboBoxLoai.SelectedItem = value;
                        }
                        else
                        {
                            comboBoxLoai.SelectedIndex = -1; // Bỏ chọn nếu không có giá trị hợp lệ
                        }
                    }
                }
            

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
