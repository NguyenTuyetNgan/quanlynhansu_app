using System.Windows;
using quanlynhansu_app.Models; // Quan trọng: Phải có dòng này để hiểu "NhanSu" là gì

namespace quanlynhansu_app.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for NhanSuDetailDialog.xaml
    /// </summary>
    public partial class NhanSuDetailDialog : Window
    {
        // 1. Constructor mặc định (Có thể giữ hoặc xóa, nhưng nên xóa nếu không dùng)
        // public NhanSuDetailDialog()
        // {
        //     InitializeComponent();
        // }

        // 2. Constructor nhận tham số (Cái bạn đang thiếu)
        public NhanSuDetailDialog(NhanSu nhanSu)
        {
            InitializeComponent();

            // Dòng này cực quan trọng: Nó đẩy dữ liệu nhận được lên giao diện
            this.DataContext = nhanSu;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}