using System.Windows;

namespace quanlynhansu_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Lớp khởi động ứng dụng
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Cấu hình EPPlus license (cho Export Excel)
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }
    }
}