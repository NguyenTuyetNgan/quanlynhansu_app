using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using quanlynhansu_app.Models;
using quanlynhansu_app.Services;
using quanlynhansu_app.Data;

namespace quanlynhansu_app.Views.Pages
{
    /// <summary>
    /// Trang quản lý danh sách nhân sự
    /// </summary>
    public partial class NhanSuPage : Page
    {
        private NhanSuService nhanSuService;
        private List<NhanSu> allNhanSu; // Lưu toàn bộ data để filter

        public NhanSuPage()
        {
            InitializeComponent();
            nhanSuService = new NhanSuService();

            LoadData();
            LoadFilters();
        }

        /// <summary>
        /// Load dữ liệu nhân sự
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Lấy toàn bộ nhân sự
                allNhanSu = nhanSuService.GetAllNhanSu();

                // Hiển thị lên DataGrid
                dgNhanSu.ItemsSource = allNhanSu;

                // Cập nhật số lượng
                txtTotalCount.Text = allNhanSu.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi load dữ liệu: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Load dữ liệu cho filter (Phòng ban, Trạng thái)
        /// </summary>
        private void LoadFilters()
        {
            try
            {
                // Load Phòng ban
                var phongBanList = DatabaseHelper.ExecuteQuery("SELECT id, ten_phong_ban FROM phong_ban ORDER BY ten_phong_ban");
                var phongBanItems = new List<ComboBoxItem>();
                phongBanItems.Add(new ComboBoxItem { Content = "Tất cả phòng ban", Tag = 0 });

                foreach (System.Data.DataRow row in phongBanList.Rows)
                {
                    phongBanItems.Add(new ComboBoxItem
                    {
                        Content = row["ten_phong_ban"].ToString(),
                        Tag = Convert.ToInt32(row["id"])
                    });
                }
                cboPhongBan.ItemsSource = phongBanItems;
                cboPhongBan.SelectedIndex = 0;

                // Load Trạng thái
                var trangThaiList = DatabaseHelper.ExecuteQuery("SELECT id, ten_trang_thai FROM trang_thai_nhan_vien ORDER BY id");
                var trangThaiItems = new List<ComboBoxItem>();
                trangThaiItems.Add(new ComboBoxItem { Content = "Tất cả trạng thái", Tag = 0 });

                foreach (System.Data.DataRow row in trangThaiList.Rows)
                {
                    trangThaiItems.Add(new ComboBoxItem
                    {
                        Content = row["ten_trang_thai"].ToString(),
                        Tag = Convert.ToInt32(row["id"])
                    });
                }
                cboTrangThai.ItemsSource = trangThaiItems;
                cboTrangThai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load filter: {ex.Message}");
            }
        }

        /// <summary>
        /// Tìm kiếm khi gõ
        /// </summary>
        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Filter khi thay đổi combo
        /// </summary>
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Áp dụng tất cả filter
        /// </summary>
        private void ApplyFilters()
        {
            if (allNhanSu == null) return;

            var filtered = allNhanSu.AsEnumerable();

            // Filter theo search keyword
            string keyword = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(keyword))
            {
                filtered = filtered.Where(ns =>
                    ns.MaNhanVien.ToLower().Contains(keyword) ||
                    ns.HoTen.ToLower().Contains(keyword) ||
                    (ns.Email != null && ns.Email.ToLower().Contains(keyword))
                );
            }

            // Filter theo phòng ban
            if (cboPhongBan.SelectedItem is ComboBoxItem pbItem && (int)pbItem.Tag > 0)
            {
                int phongBanId = (int)pbItem.Tag;
                filtered = filtered.Where(ns => ns.PhongBanId == phongBanId);
            }

            // Filter theo trạng thái
            if (cboTrangThai.SelectedItem is ComboBoxItem ttItem && (int)ttItem.Tag > 0)
            {
                int trangThaiId = (int)ttItem.Tag;
                filtered = filtered.Where(ns => ns.TrangThaiId == trangThaiId);
            }

            // Cập nhật DataGrid
            var result = filtered.ToList();
            dgNhanSu.ItemsSource = result;
            txtTotalCount.Text = result.Count.ToString();
        }

        /// <summary>
        /// Reset tất cả filter
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            cboPhongBan.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            dgNhanSu.ItemsSource = allNhanSu;
            txtTotalCount.Text = allNhanSu.Count.ToString();
        }

        /// <summary>
        /// Thêm nhân sự mới
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.NhanSuDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadData(); // Reload sau khi thêm
            }
        }

        /// <summary>
        /// Xem chi tiết nhân sự
        /// </summary>
        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var nhanSu = nhanSuService.GetNhanSuById(id);
                if (nhanSu != null)
                {
                    var dialog = new Dialogs.NhanSuDetailDialog(nhanSu);
                    dialog.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Chỉnh sửa nhân sự
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var nhanSu = nhanSuService.GetNhanSuById(id);
                if (nhanSu != null)
                {
                    var dialog = new Dialogs.NhanSuDialog(nhanSu);
                    if (dialog.ShowDialog() == true)
                    {
                        LoadData(); // Reload sau khi sửa
                    }
                }
            }
        }

        /// <summary>
        /// Xóa nhân sự
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc muốn xóa nhân sự này?\n\nHành động này không thể hoàn tác!",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool success = nhanSuService.DeleteNhanSu(id);
                        if (success)
                        {
                            MessageBox.Show(
                                "Xóa nhân sự thành công!",
                                "Thành công",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Lỗi khi xóa: {ex.Message}",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Import Excel
        /// </summary>
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng Import Excel sẽ được phát triển sau.\n\nSử dụng thư viện EPPlus hoặc ClosedXML.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng Export Excel sẽ được phát triển sau.\n\nSử dụng thư viện EPPlus hoặc ClosedXML.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Sự kiện khi chọn dòng trong DataGrid
        /// </summary>
        private void DgNhanSu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Có thể xử lý thêm khi chọn dòng
        }
    }
}