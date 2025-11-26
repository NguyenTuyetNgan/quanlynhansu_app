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
    /// Dialog để thêm hoặc sửa thông tin nhân sự
    /// </summary>
    public partial class NhanSuDialog : Window
    {
        private NhanSu _nhanSu;
        private bool _isEditMode;
        private NhanSuService _nhanSuService;

        /// <summary>
        /// Constructor cho chế độ THÊM MỚI
        /// </summary>
        public NhanSuDialog()
        {
            InitializeComponent();

            _isEditMode = false;
            _nhanSuService = new NhanSuService();
            _nhanSu = new NhanSu();

            txtTitle.Text = "➕ Thêm nhân sự mới";

            LoadComboBoxData();
        }

        /// <summary>
        /// Constructor cho chế độ CHỈNH SỬA
        /// </summary>
        public NhanSuDialog(NhanSu nhanSu)
        {
            InitializeComponent();

            _isEditMode = true;
            _nhanSuService = new NhanSuService();
            _nhanSu = nhanSu;

            txtTitle.Text = "✏️ Chỉnh sửa thông tin nhân sự";

            LoadComboBoxData();
            LoadNhanSuData();
        }

        /// <summary>
        /// Load dữ liệu cho các ComboBox
        /// </summary>
        private void LoadComboBoxData()
        {
            try
            {
                // Load Chức vụ
                var chucVuList = DatabaseHelper.ExecuteQuery("SELECT id, ten_chuc_vu FROM chuc_vu ORDER BY ten_chuc_vu");
                var chucVuItems = new List<ComboBoxItem>();
                chucVuItems.Add(new ComboBoxItem { Content = "-- Chọn chức vụ --", Tag = 0 });
                foreach (System.Data.DataRow row in chucVuList.Rows)
                {
                    chucVuItems.Add(new ComboBoxItem
                    {
                        Content = row["ten_chuc_vu"].ToString(),
                        Tag = Convert.ToInt32(row["id"])
                    });
                }
                cboChucVu.ItemsSource = chucVuItems;
                cboChucVu.SelectedIndex = 0;

                // Load Phòng ban
                var phongBanList = DatabaseHelper.ExecuteQuery("SELECT id, ten_phong_ban FROM phong_ban ORDER BY ten_phong_ban");
                var phongBanItems = new List<ComboBoxItem>();
                phongBanItems.Add(new ComboBoxItem { Content = "-- Chọn phòng ban --", Tag = 0 });
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

                // Load Loại hợp đồng
                var hopDongList = DatabaseHelper.ExecuteQuery("SELECT id, ten_loai FROM loai_hop_dong ORDER BY ten_loai");
                var hopDongItems = new List<ComboBoxItem>();
                hopDongItems.Add(new ComboBoxItem { Content = "-- Chọn loại hợp đồng --", Tag = 0 });
                foreach (System.Data.DataRow row in hopDongList.Rows)
                {
                    hopDongItems.Add(new ComboBoxItem
                    {
                        Content = row["ten_loai"].ToString(),
                        Tag = Convert.ToInt32(row["id"])
                    });
                }
                cboLoaiHopDong.ItemsSource = hopDongItems;
                cboLoaiHopDong.SelectedIndex = 0;

                // Load Trình độ học vấn
                var trinhDoList = DatabaseHelper.ExecuteQuery("SELECT id, ten_trinh_do FROM trinh_do_hoc_van ORDER BY ten_trinh_do");
                var trinhDoItems = new List<ComboBoxItem>();
                trinhDoItems.Add(new ComboBoxItem { Content = "-- Chọn trình độ --", Tag = 0 });
                foreach (System.Data.DataRow row in trinhDoList.Rows)
                {
                    trinhDoItems.Add(new ComboBoxItem
                    {
                        Content = row["ten_trinh_do"].ToString(),
                        Tag = Convert.ToInt32(row["id"])
                    });
                }
                cboTrinhDo.ItemsSource = trinhDoItems;
                cboTrinhDo.SelectedIndex = 0;

                // Load Trạng thái
                var trangThaiList = DatabaseHelper.ExecuteQuery("SELECT id, ten_trang_thai FROM trang_thai_nhan_vien ORDER BY id");
                var trangThaiItems = new List<ComboBoxItem>();
                foreach (System.Data.DataRow row in trangThaiList.Rows)
                {
                    trangThaiItems.Add(new ComboBoxItem
                    {
                        Content = row["ten_trang_thai"].ToString(),
                        Tag = Convert.ToInt32(row["id"])
                    });
                }
                cboTrangThai.ItemsSource = trangThaiItems;
                cboTrangThai.SelectedIndex = 0; // Mặc định "Đang làm việc"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu ComboBox: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Load thông tin nhân sự lên form (chế độ Edit)
        /// </summary>
        private void LoadNhanSuData()
        {
            if (_nhanSu == null) return;

            txtMaNhanVien.Text = _nhanSu.MaNhanVien;
            txtHoTen.Text = _nhanSu.HoTen;
            dpNgaySinh.SelectedDate = _nhanSu.NgaySinh;

            // Giới tính
            if (_nhanSu.GioiTinh == "Nam")
                cboGioiTinh.SelectedIndex = 0;
            else if (_nhanSu.GioiTinh == "Nữ")
                cboGioiTinh.SelectedIndex = 1;
            else
                cboGioiTinh.SelectedIndex = 2;

            txtSoDienThoai.Text = _nhanSu.SoDienThoai;
            txtEmail.Text = _nhanSu.Email;
            txtDiaChi.Text = _nhanSu.DiaChi;
            dpNgayVaoLam.SelectedDate = _nhanSu.NgayVaoLam;
            txtMucLuong.Text = _nhanSu.MucLuong?.ToString();

            // Set selected cho các ComboBox
            SelectComboBoxItem(cboChucVu, _nhanSu.ChucVuId);
            SelectComboBoxItem(cboPhongBan, _nhanSu.PhongBanId);
            SelectComboBoxItem(cboLoaiHopDong, _nhanSu.LoaiHopDongId);
            SelectComboBoxItem(cboTrinhDo, _nhanSu.TrinhDoHocVanId);
            SelectComboBoxItem(cboTrangThai, _nhanSu.TrangThaiId);
        }

        /// <summary>
        /// Helper method để select item trong ComboBox theo ID
        /// </summary>
        private void SelectComboBoxItem(ComboBox comboBox, int? id)
        {
            if (!id.HasValue || id == 0)
            {
                comboBox.SelectedIndex = 0;
                return;
            }

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Tag != null && (int)item.Tag == id.Value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Lưu thông tin
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate dữ liệu
            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMaNhanVien.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtHoTen.Focus();
                return;
            }

            if (cboChucVu.SelectedItem is ComboBoxItem cvItem && (int)cvItem.Tag == 0)
            {
                MessageBox.Show("Vui lòng chọn chức vụ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cboPhongBan.SelectedItem is ComboBoxItem pbItem && (int)pbItem.Tag == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng ban!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Lấy dữ liệu từ form
                _nhanSu.MaNhanVien = txtMaNhanVien.Text.Trim();
                _nhanSu.HoTen = txtHoTen.Text.Trim();
                _nhanSu.NgaySinh = dpNgaySinh.SelectedDate;
                _nhanSu.GioiTinh = ((ComboBoxItem)cboGioiTinh.SelectedItem).Content.ToString();
                _nhanSu.SoDienThoai = txtSoDienThoai.Text.Trim();
                _nhanSu.Email = txtEmail.Text.Trim();
                _nhanSu.DiaChi = txtDiaChi.Text.Trim();
                _nhanSu.NgayVaoLam = dpNgayVaoLam.SelectedDate;

                // Parse mức lương
                if (!string.IsNullOrWhiteSpace(txtMucLuong.Text))
                {
                    if (decimal.TryParse(txtMucLuong.Text, out decimal luong))
                        _nhanSu.MucLuong = luong;
                }

                // Lấy ID từ ComboBox
                _nhanSu.ChucVuId = GetComboBoxSelectedValue(cboChucVu);
                _nhanSu.PhongBanId = GetComboBoxSelectedValue(cboPhongBan);
                _nhanSu.LoaiHopDongId = GetComboBoxSelectedValue(cboLoaiHopDong);
                _nhanSu.TrinhDoHocVanId = GetComboBoxSelectedValue(cboTrinhDo);
                _nhanSu.TrangThaiId = GetComboBoxSelectedValue(cboTrangThai) ?? 1;

                // Gọi service để lưu
                bool success;
                if (_isEditMode)
                {
                    success = _nhanSuService.UpdateNhanSu(_nhanSu);
                    if (success)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    success = _nhanSuService.AddNhanSu(_nhanSu);
                    if (success)
                    {
                        MessageBox.Show("Thêm nhân sự thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (success)
                {
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Helper method lấy giá trị selected từ ComboBox
        /// </summary>
        private int? GetComboBoxSelectedValue(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                int value = (int)item.Tag;
                return value == 0 ? null : (int?)value;
            }
            return null;
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