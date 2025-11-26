using System.Windows;
using System.Windows.Controls;
using quanlynhansu_app.Services;

namespace quanlynhansu_app.Views.Dialogs
{
    public partial class AddUserDialog : Window
    {
        private TaiKhoanService _service;

        public AddUserDialog()
        {
            InitializeComponent();
            _service = new TaiKhoanService();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Password;
            string email = txtEmail.Text.Trim();
            string role = ((ComboBoxItem)cboRole.SelectedItem).Tag.ToString();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!");
                return;
            }

            if (_service.CreateUser(user, pass, email, role))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Có thể tên đăng nhập đã tồn tại.");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}