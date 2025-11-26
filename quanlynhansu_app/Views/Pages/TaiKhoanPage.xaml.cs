using System.Windows;
using System.Windows.Controls;
using quanlynhansu_app.Services;

namespace quanlynhansu_app.Views.Pages
{
    public partial class TaiKhoanPage : Page
    {
        private TaiKhoanService _service;

        public TaiKhoanPage()
        {
            InitializeComponent();
            _service = new TaiKhoanService();
            LoadData();
        }

        private void LoadData()
        {
            dgTaiKhoan.ItemsSource = _service.GetAllUsers();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.AddUserDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadData(); // Load lại danh sách sau khi thêm
            }
        }

        private void BtnResetPass_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (MessageBox.Show("Bạn muốn đặt lại mật khẩu về mặc định '123'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _service.ResetPassword(id, "123");
                    MessageBox.Show("Đã reset mật khẩu thành công!");
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (MessageBox.Show("Xóa tài khoản này?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _service.DeleteUser(id);
                    LoadData();
                }
            }
        }
    }
}