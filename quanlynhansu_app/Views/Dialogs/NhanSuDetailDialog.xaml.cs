using System; // Thêm dòng này để dùng AppDomain, Exception
using System.Windows;
using quanlynhansu_app.Models;

namespace quanlynhansu_app.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for NhanSuDetailDialog.xaml
    /// </summary>
    public partial class NhanSuDetailDialog : Window
    {
        // Khai báo biến để lưu thông tin nhân sự hiện tại
        private NhanSu _nhanSu;

        public NhanSuDetailDialog(NhanSu nhanSu)
        {
            InitializeComponent();

            _nhanSu = nhanSu; // Lưu dữ liệu vào biến
            this.DataContext = nhanSu; // Đẩy dữ liệu lên giao diện
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string sourceFile = openFileDialog.FileName;
                    string fileName = System.IO.Path.GetFileName(sourceFile);

                    // Tạo folder lưu trữ: bin/Debug/net8.0-windows/Uploads
                    string destFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                    if (!System.IO.Directory.Exists(destFolder))
                        System.IO.Directory.CreateDirectory(destFolder);

                    string destFile = System.IO.Path.Combine(destFolder, fileName);

                    // Copy file (ghi đè nếu trùng tên)
                    System.IO.File.Copy(sourceFile, destFile, true);

                    // TODO: Gọi Service lưu vào DB ở đây
                    // var service = new Services.NhanSuService();
                    // service.AddTaiLieu(_nhanSu.Id, fileName, destFile);

                    MessageBox.Show($"Upload file '{fileName}' thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi upload: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}