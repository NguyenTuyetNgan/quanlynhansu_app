using System;
using System.Windows;
using quanlynhansu_app.Views.Pages;

namespace quanlynhansu_app
{
    /// <summary>
    /// Cửa sổ chính của ứng dụng
    /// Chứa top navbar và Frame để navigate giữa các Page
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load thông tin user từ Application Properties
            LoadUserInfo();

            // Mặc định hiển thị Dashboard
            MainFrame.Navigate(new DashboardPage());
        }

        /// <summary>
        /// Load thông tin user lên navbar
        /// </summary>
        private void LoadUserInfo()
        {
            try
            {
                if (Application.Current.Properties["Username"] != null)
                {
                    txtUsername.Text = Application.Current.Properties["Username"].ToString();
                }

                if (Application.Current.Properties["Role"] != null)
                {
                    string role = Application.Current.Properties["Role"].ToString();
                    txtRole.Text = role == "admin" ? "Quản trị viên" : "Nhân viên";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thông tin user: {ex.Message}");
            }
        }

        /// <summary>
        /// Navigate đến Dashboard
        /// </summary>
        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage());
        }

        /// <summary>
        /// Navigate đến trang Nhân sự
        /// </summary>
        private void BtnNhanSu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NhanSuPage());
        }

        /// <summary>
        /// Navigate đến trang Phòng ban
        /// </summary>
        private void BtnPhongBan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PhongBanPage());
        }

        /// <summary>
        /// Navigate đến trang Danh mục
        /// </summary>
        private void BtnDanhMuc_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DanhMucPage());
        }

        private void BtnTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TaiKhoanPage());
        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Clear user properties
                Application.Current.Properties.Clear();

                // Mở lại LoginWindow
                Views.LoginWindow loginWindow = new Views.LoginWindow();
                loginWindow.Show();

                // Đóng MainWindow
                this.Close();
            }
        }
    }
}