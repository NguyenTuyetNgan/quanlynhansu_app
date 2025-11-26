using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using quanlynhansu_app.Services; // Nhớ đổi tên namespace nếu khác
using quanlynhansu_app.Models;

namespace quanlynhansu_app.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private NhanSuService _nhanSuService;
        private PhongBanService _phongBanService;

        public DashboardPage()
        {
            InitializeComponent();

            // Khởi tạo Service
            _nhanSuService = new NhanSuService();
            _phongBanService = new PhongBanService();

            // Load dữ liệu khi mở trang
            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDashboardData();
        }

        /// <summary>
        /// Hàm load toàn bộ dữ liệu thống kê
        /// </summary>
        private void LoadDashboardData()
        {
            try
            {
                // 1. Lấy dữ liệu thô từ DB
                var listNhanSu = _nhanSuService.GetAllNhanSu(); // Bạn cần đảm bảo hàm này có trong Service
                var listPhongBan = _phongBanService.GetAllPhongBan();

                // 2. Điền số liệu tổng quan (4 Card trên cùng)
                txtTongNhanSu.Text = listNhanSu.Count.ToString();
                txtTongPhongBan.Text = listPhongBan.Count.ToString();

                // Nhân sự mới (ví dụ: vào làm trong tháng này)
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                int newEmpCount = listNhanSu.Count(ns => ns.NgayVaoLam.HasValue &&
                                                        ns.NgayVaoLam.Value.Month == currentMonth &&
                                                        ns.NgayVaoLam.Value.Year == currentYear);
                txtNhanSuMoi.Text = newEmpCount.ToString();

                // Sinh nhật tháng này
                txtThangHienTai.Text = currentMonth.ToString();
                int birthdayCount = listNhanSu.Count(ns => ns.NgaySinh.HasValue &&
                                                         ns.NgaySinh.Value.Month == currentMonth);
                txtSinhNhat.Text = birthdayCount.ToString();


                // 3. Cấu hình Biểu đồ Cột (Phòng ban) - LiveCharts v2
                // Logic: Group nhân viên theo PhongBanId, đếm số lượng
                var phongBanStats = listNhanSu.GroupBy(ns => ns.PhongBanId)
                                              .Select(g => new { Id = g.Key, Count = g.Count() })
                                              .ToList();

                var columnSeries = new ColumnSeries<int>
                {
                    Values = phongBanStats.Select(x => x.Count).ToArray(),
                    Name = "Nhân sự",
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue)
                };

                chartPhongBan.Series = new ISeries[] { columnSeries };

                // Trục X (Tên phòng ban)
                chartPhongBan.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = listPhongBan.Select(pb => pb.TenPhongBan).ToArray(),
                        LabelsRotation = 0
                    }
                };


                // 4. Cấu hình Biểu đồ Tròn (Giới tính)
                int nam = listNhanSu.Count(x => x.GioiTinh == "Nam");
                int nu = listNhanSu.Count(x => x.GioiTinh == "Nữ");

                chartGioiTinh.Series = new ISeries[]
                {
                    new PieSeries<int> { Values = new int[] { nam }, Name = "Nam", InnerRadius = 50, Fill = new SolidColorPaint(SKColors.Blue) },
                    new PieSeries<int> { Values = new int[] { nu }, Name = "Nữ", InnerRadius = 50, Fill = new SolidColorPaint(SKColors.Pink) }
                };


                // 5. Load danh sách thống kê chi tiết (ItemsControl)
                LoadDetailedStats(listNhanSu);

            }
            catch (Exception ex)
            {
                // Tạm thời bỏ qua lỗi để không crash app nếu data chưa có
                Console.WriteLine("Lỗi load dashboard: " + ex.Message);
            }
        }

        private void LoadDetailedStats(List<NhanSu> listNhanSu)
        {
            // --- Thống kê Chức vụ ---
            // Giả lập dữ liệu hoặc lấy từ DB. Ở đây mình demo logic tính toán
            var chucVuData = listNhanSu.GroupBy(x => x.ChucVuId)
                                       .Select(g => new ThongKeItem
                                       {
                                           Ten = $"Chức vụ {g.Key}", // Nên lấy tên thật từ bảng ChucVu
                                           SoLuong = g.Count(),
                                           Total = listNhanSu.Count
                                       }).OrderByDescending(x => x.SoLuong).Take(5).ToList();
            lstChucVu.ItemsSource = chucVuData;

            // --- Thống kê Hợp đồng ---
            var hopDongData = listNhanSu.GroupBy(x => x.LoaiHopDongId)
                                        .Select(g => new ThongKeItem
                                        {
                                            Ten = $"Hợp đồng {g.Key}",
                                            SoLuong = g.Count(),
                                            Total = listNhanSu.Count
                                        }).ToList();
            lstHopDong.ItemsSource = hopDongData;

            // --- Thống kê Trạng thái ---
            var trangThaiData = listNhanSu.GroupBy(x => x.TrangThaiId)
                                          .Select(g => new ThongKeItem
                                          {
                                              Ten = $"Trạng thái {g.Key}",
                                              SoLuong = g.Count(),
                                              Total = listNhanSu.Count
                                          }).ToList();
            lstTrangThai.ItemsSource = trangThaiData;
        }

        // ==========================================================
        // ĐÂY LÀ HÀM BẠN ĐANG BỊ THIẾU GÂY RA LỖI
        // ==========================================================
        private void BtnExportReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng xuất báo cáo đang được phát triển!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    /// <summary>
    /// Class phụ trợ để hiển thị lên ItemsControl (Binding)
    /// </summary>
    public class ThongKeItem
    {
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int Total { get; set; }

        // Tính độ rộng thanh bar (trên thang 200px)
        public double Width
        {
            get
            {
                if (Total == 0) return 0;
                return (double)SoLuong / Total * 150; // 150 là max width của thanh bar
            }
        }
    }
}