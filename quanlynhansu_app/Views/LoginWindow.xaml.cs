using MySql.Data.MySqlClient;
using quanlynhansu_app.Data;
using System;
using System.Windows;
using System.Windows.Input;

namespace quanlynhansu_app.Views
{
    /// <summary>
    /// Cửa sổ đăng nhập
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Kiểm tra kết nối database khi khởi động
            if (!DatabaseHelper.TestConnection())
            {
                MessageBox.Show(
                    "Không thể kết nối đến database!\n\n" +
                    "Vui lòng kiểm tra:\n" +
                    "1. MySQL Server đã chạy chưa\n" +
                    "2. Database 'quanlynhansu_db' đã tạo chưa\n" +
                    "3. Connection string trong DatabaseHelper.cs",
                    "Lỗi kết nối",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút đăng nhập
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        /// <summary>
        /// Cho phép nhấn Enter để đăng nhập
        /// </summary>
        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
            }
        }

        /// <summary>
        /// Thực hiện đăng nhập
        /// </summary>
        private void PerformLogin()
        {
            // Lấy thông tin từ form
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            // Validate
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            try
            {
                // Query kiểm tra user
                string query = "SELECT id, username, email, role, nhan_su_id FROM users WHERE username = @Username AND password = @Password";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@Username", username),
                    new MySqlParameter("@Password", password) // Lưu ý: Trong thực tế nên hash password
                };

                var dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    // Đăng nhập thành công
                    var row = dt.Rows[0];

                    // Lưu thông tin user vào Application Properties để dùng trong toàn app
                    Application.Current.Properties["UserId"] = Convert.ToInt32(row["id"]);
                    Application.Current.Properties["Username"] = row["username"].ToString();
                    Application.Current.Properties["Role"] = row["role"].ToString();
                    Application.Current.Properties["NhanSuId"] = row["nhan_su_id"] != DBNull.Value ?
                        Convert.ToInt32(row["nhan_su_id"]) : (int?)null;

                    // Mở MainWindow
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    // Đóng LoginWindow
                    this.Close();
                }
                else
                {
                    // Sai thông tin đăng nhập
                    ShowError("Tên đăng nhập hoặc mật khẩu không đúng!");
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi đăng nhập: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        private void ShowError(string message)
        {
            txtError.Text = "✗ " + message;
            txtError.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Đóng cửa sổ
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}