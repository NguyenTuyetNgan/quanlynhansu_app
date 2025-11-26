using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using quanlynhansu_app.Data;
using quanlynhansu_app.Models;
using quanlynhansu_app.Services;

namespace quanlynhansu_app.Views.Dialogs
{
    /// <summary>
    /// Dialog để thêm hoặc sửa thông tin phòng ban
    /// </summary>
    public partial class PhongBanDialog : Window
    {
        private PhongBan _phongBan;
        private bool _isEditMode;
        private PhongBanService _phongBanService;

        /// <summary>
        /// Constructor cho chế độ THÊM MỚI
        /// </summary>
        public PhongBanDialog()
        {
            InitializeComponent();

            _isEditMode = false;
            _phongBanService = new PhongBanService();
            _phongBan = new PhongBan();

            txtTitle.Text = "➕ Thêm phòng ban mới";

            LoadTruongPhongComboBox();
        }

        /// <summary>
        /// Constructor cho chế độ CHỈNH SỬA
        /// </summary>
        public PhongBanDialog(PhongBan phongBan)
        {
            InitializeComponent();

            _isEditMode = true;
            _phongBanService = new PhongBanService();
            _phongBan = phongBan;

            txtTitle.Text = "✏️ Chỉnh sửa thông tin phòng ban";

            LoadTruongPhongComboBox();
            LoadPhongBanData();
        }

        /// <summary>
        /// Load danh sách nhân viên cho ComboBox Trưởng phòng
        /// Chỉ lấy những nhân viên đang làm việc
        /// </summary>
        private void LoadTruongPhongComboBox()
        {
            try
            {
                // Lấy danh sách nhân viên đang làm việc
                var nhanSuList = DatabaseHelper.ExecuteQuery(
                    @"SELECT id, ma_nhan_vien, ho_ten 
                      FROM nhan_su 
                      WHERE trang_thai_id = 1 
                      ORDER BY ho_ten");

                var items = new List<ComboBoxItem>();
                items.Add(new ComboBoxItem
                {
                    Content = "-- Chưa có trưởng phòng --",
                    Tag = 0
                });

                foreach (System.Data.DataRow row in nhanSuList.Rows)
                {
                    string displayText = $"{row["ho_ten"]} ({row["ma_nhan_vien"]})";
                    items.Add(new ComboBoxItem
                    {
                        Content = displayText,
                        Tag = Convert.ToInt32(row["id"])
                    });
                }

                cboTruongPhong.ItemsSource = items;
                cboTruongPhong.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi load danh sách nhân viên: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Load thông tin phòng ban lên form (chế độ Edit)
        /// </summary>
        private void LoadPhongBanData()
        {
            if (_phongBan == null) return;

            txtMaPhongBan.Text = _phongBan.MaPhongBan;
            txtTenPhongBan.Text = _phongBan.TenPhongBan;
            txtMoTa.Text = _phongBan.MoTa;

            // Set selected cho ComboBox Trưởng phòng
            if (_phongBan.TruongPhong.HasValue)
            {
                foreach (ComboBoxItem item in cboTruongPhong.Items)
                {
                    if (item.Tag != null && (int)item.Tag == _phongBan.TruongPhong.Value)
                    {
                        cboTruongPhong.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                cboTruongPhong.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Lưu thông tin phòng ban
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate dữ liệu
            if (string.IsNullOrWhiteSpace(txtMaPhongBan.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập mã phòng ban!",
                    "Cảnh báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtMaPhongBan.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenPhongBan.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên phòng ban!",
                    "Cảnh báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtTenPhongBan.Focus();
                return;
            }

            try
            {
                // Lấy dữ liệu từ form
                _phongBan.MaPhongBan = txtMaPhongBan.Text.Trim();
                _phongBan.TenPhongBan = txtTenPhongBan.Text.Trim();
                _phongBan.MoTa = txtMoTa.Text.Trim();

                // Lấy ID trưởng phòng từ ComboBox
                if (cboTruongPhong.SelectedItem is ComboBoxItem item && item.Tag != null)
                {
                    int truongPhongId = (int)item.Tag;
                    _phongBan.TruongPhong = truongPhongId == 0 ? null : (int?)truongPhongId;
                }

                // Gọi service để lưu
                bool success;
                if (_isEditMode)
                {
                    success = _phongBanService.UpdatePhongBan(_phongBan);
                    if (success)
                    {
                        MessageBox.Show(
                            "Cập nhật thông tin phòng ban thành công!",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Kiểm tra mã phòng ban đã tồn tại chưa
                    var existingQuery = "SELECT COUNT(*) FROM phong_ban WHERE ma_phong_ban = @MaPhongBan";
                    var existingParams = new MySql.Data.MySqlClient.MySqlParameter[]
                    {
                        new MySql.Data.MySqlClient.MySqlParameter("@MaPhongBan", _phongBan.MaPhongBan)
                    };
                    var count = DatabaseHelper.ExecuteScalar(existingQuery, existingParams);

                    if (Convert.ToInt32(count) > 0)
                    {
                        MessageBox.Show(
                            $"Mã phòng ban '{_phongBan.MaPhongBan}' đã tồn tại!\n\nVui lòng sử dụng mã khác.",
                            "Cảnh báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        txtMaPhongBan.Focus();
                        txtMaPhongBan.SelectAll();
                        return;
                    }

                    success = _phongBanService.AddPhongBan(_phongBan);
                    if (success)
                    {
                        MessageBox.Show(
                            "Thêm phòng ban thành công!",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }

                if (success)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không thể lưu thông tin phòng ban!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi lưu: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Hủy và đóng dialog
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}