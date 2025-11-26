using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using quanlynhansu_app.Data;
using quanlynhansu_app.Models;

namespace quanlynhansu_app.Views.Pages
{
    /// <summary>
    /// Trang quản lý các danh mục hệ thống
    /// Bao gồm: Chức vụ, Loại hợp đồng, Trình độ, Trạng thái, Loại tài liệu
    /// </summary>
    public partial class DanhMucPage : Page
    {
        public DanhMucPage()
        {
            InitializeComponent();
            LoadAllData();
        }

        /// <summary>
        /// Load tất cả dữ liệu danh mục
        /// </summary>
        private void LoadAllData()
        {
            LoadChucVu();
            LoadLoaiHopDong();
            LoadTrinhDo();
            LoadTrangThai();
            LoadLoaiTaiLieu();
        }

        // ==================== CHỨC VỤ ====================

        private void LoadChucVu()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM chuc_vu ORDER BY ten_chuc_vu");
                var list = new System.Collections.Generic.List<ChucVu>();

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    list.Add(new ChucVu
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenChucVu = row["ten_chuc_vu"].ToString()
                    });
                }

                lstChucVu.ItemsSource = list;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi load chức vụ", ex.Message);
            }
        }

        private void BtnAddChucVu_Click(object sender, RoutedEventArgs e)
        {
            var input = ShowInputDialog("Thêm chức vụ", "Nhập tên chức vụ:");
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    var query = "INSERT INTO chuc_vu (ten_chuc_vu) VALUES (@Ten)";
                    var param = new MySqlParameter("@Ten", input.Trim());
                    DatabaseHelper.ExecuteNonQuery(query, param);

                    ShowSuccess("Thêm chức vụ thành công!");
                    LoadChucVu();
                }
                catch (Exception ex)
                {
                    ShowError("Lỗi thêm chức vụ", ex.Message);
                }
            }
        }

        private void BtnDeleteChucVu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (ConfirmDelete("chức vụ"))
                {
                    try
                    {
                        var query = "DELETE FROM chuc_vu WHERE id = @Id";
                        var param = new MySqlParameter("@Id", id);
                        DatabaseHelper.ExecuteNonQuery(query, param);

                        ShowSuccess("Xóa chức vụ thành công!");
                        LoadChucVu();
                    }
                    catch (Exception ex)
                    {
                        ShowError("Không thể xóa", "Chức vụ này đang được sử dụng!");
                    }
                }
            }
        }

        // ==================== LOẠI HỢP ĐỒNG ====================

        private void LoadLoaiHopDong()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM loai_hop_dong ORDER BY ten_loai");
                var list = new System.Collections.Generic.List<LoaiHopDong>();

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    list.Add(new LoaiHopDong
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenLoai = row["ten_loai"].ToString()
                    });
                }

                lstLoaiHopDong.ItemsSource = list;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi load loại hợp đồng", ex.Message);
            }
        }

        private void BtnAddLoaiHopDong_Click(object sender, RoutedEventArgs e)
        {
            var input = ShowInputDialog("Thêm loại hợp đồng", "Nhập tên loại hợp đồng:");
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    var query = "INSERT INTO loai_hop_dong (ten_loai) VALUES (@Ten)";
                    var param = new MySqlParameter("@Ten", input.Trim());
                    DatabaseHelper.ExecuteNonQuery(query, param);

                    ShowSuccess("Thêm loại hợp đồng thành công!");
                    LoadLoaiHopDong();
                }
                catch (Exception ex)
                {
                    ShowError("Lỗi thêm loại hợp đồng", ex.Message);
                }
            }
        }

        private void BtnDeleteLoaiHopDong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (ConfirmDelete("loại hợp đồng"))
                {
                    try
                    {
                        var query = "DELETE FROM loai_hop_dong WHERE id = @Id";
                        var param = new MySqlParameter("@Id", id);
                        DatabaseHelper.ExecuteNonQuery(query, param);

                        ShowSuccess("Xóa loại hợp đồng thành công!");
                        LoadLoaiHopDong();
                    }
                    catch (Exception ex)
                    {
                        ShowError("Không thể xóa", "Loại hợp đồng này đang được sử dụng!");
                    }
                }
            }
        }

        // ==================== TRÌNH ĐỘ HỌC VẤN ====================

        private void LoadTrinhDo()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM trinh_do_hoc_van ORDER BY ten_trinh_do");
                var list = new System.Collections.Generic.List<TrinhDoHocVan>();

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    list.Add(new TrinhDoHocVan
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenTrinhDo = row["ten_trinh_do"].ToString()
                    });
                }

                lstTrinhDo.ItemsSource = list;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi load trình độ", ex.Message);
            }
        }

        private void BtnAddTrinhDo_Click(object sender, RoutedEventArgs e)
        {
            var input = ShowInputDialog("Thêm trình độ", "Nhập tên trình độ học vấn:");
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    var query = "INSERT INTO trinh_do_hoc_van (ten_trinh_do) VALUES (@Ten)";
                    var param = new MySqlParameter("@Ten", input.Trim());
                    DatabaseHelper.ExecuteNonQuery(query, param);

                    ShowSuccess("Thêm trình độ thành công!");
                    LoadTrinhDo();
                }
                catch (Exception ex)
                {
                    ShowError("Lỗi thêm trình độ", ex.Message);
                }
            }
        }

        private void BtnDeleteTrinhDo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (ConfirmDelete("trình độ"))
                {
                    try
                    {
                        var query = "DELETE FROM trinh_do_hoc_van WHERE id = @Id";
                        var param = new MySqlParameter("@Id", id);
                        DatabaseHelper.ExecuteNonQuery(query, param);

                        ShowSuccess("Xóa trình độ thành công!");
                        LoadTrinhDo();
                    }
                    catch (Exception ex)
                    {
                        ShowError("Không thể xóa", "Trình độ này đang được sử dụng!");
                    }
                }
            }
        }

        // ==================== TRẠNG THÁI NHÂN VIÊN ====================

        private void LoadTrangThai()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM trang_thai_nhan_vien ORDER BY id");
                var list = new System.Collections.Generic.List<TrangThaiNhanVien>();

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    list.Add(new TrangThaiNhanVien
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenTrangThai = row["ten_trang_thai"].ToString()
                    });
                }

                lstTrangThai.ItemsSource = list;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi load trạng thái", ex.Message);
            }
        }

        private void BtnAddTrangThai_Click(object sender, RoutedEventArgs e)
        {
            var input = ShowInputDialog("Thêm trạng thái", "Nhập tên trạng thái:");
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    var query = "INSERT INTO trang_thai_nhan_vien (ten_trang_thai) VALUES (@Ten)";
                    var param = new MySqlParameter("@Ten", input.Trim());
                    DatabaseHelper.ExecuteNonQuery(query, param);

                    ShowSuccess("Thêm trạng thái thành công!");
                    LoadTrangThai();
                }
                catch (Exception ex)
                {
                    ShowError("Lỗi thêm trạng thái", ex.Message);
                }
            }
        }

        private void BtnDeleteTrangThai_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (ConfirmDelete("trạng thái"))
                {
                    try
                    {
                        var query = "DELETE FROM trang_thai_nhan_vien WHERE id = @Id";
                        var param = new MySqlParameter("@Id", id);
                        DatabaseHelper.ExecuteNonQuery(query, param);

                        ShowSuccess("Xóa trạng thái thành công!");
                        LoadTrangThai();
                    }
                    catch (Exception ex)
                    {
                        ShowError("Không thể xóa", "Trạng thái này đang được sử dụng!");
                    }
                }
            }
        }

        // ==================== LOẠI TÀI LIỆU ====================

        private void LoadLoaiTaiLieu()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM loai_tai_lieu ORDER BY ten_loai");
                var list = new System.Collections.Generic.List<LoaiTaiLieu>();

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    list.Add(new LoaiTaiLieu
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenLoai = row["ten_loai"].ToString()
                    });
                }

                lstLoaiTaiLieu.ItemsSource = list;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi load loại tài liệu", ex.Message);
            }
        }

        private void BtnAddLoaiTaiLieu_Click(object sender, RoutedEventArgs e)
        {
            var input = ShowInputDialog("Thêm loại tài liệu", "Nhập tên loại tài liệu:");
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    var query = "INSERT INTO loai_tai_lieu (ten_loai) VALUES (@Ten)";
                    var param = new MySqlParameter("@Ten", input.Trim());
                    DatabaseHelper.ExecuteNonQuery(query, param);

                    ShowSuccess("Thêm loại tài liệu thành công!");
                    LoadLoaiTaiLieu();
                }
                catch (Exception ex)
                {
                    ShowError("Lỗi thêm loại tài liệu", ex.Message);
                }
            }
        }

        private void BtnDeleteLoaiTaiLieu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (ConfirmDelete("loại tài liệu"))
                {
                    try
                    {
                        var query = "DELETE FROM loai_tai_lieu WHERE id = @Id";
                        var param = new MySqlParameter("@Id", id);
                        DatabaseHelper.ExecuteNonQuery(query, param);

                        ShowSuccess("Xóa loại tài liệu thành công!");
                        LoadLoaiTaiLieu();
                    }
                    catch (Exception ex)
                    {
                        ShowError("Không thể xóa", "Loại tài liệu này đang được sử dụng!");
                    }
                }
            }
        }

        // ==================== HELPER METHODS ====================

        /// <summary>
        /// Hiển thị dialog nhập liệu đơn giản
        /// </summary>
        private string ShowInputDialog(string title, string prompt)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var stack = new StackPanel { Margin = new Thickness(20) };

            stack.Children.Add(new TextBlock
            {
                Text = prompt,
                Margin = new Thickness(0, 0, 0, 10),
                FontSize = 14
            });

            var textBox = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 20),
                Padding = new Thickness(5), // Sửa lại thành 1 số hoặc 4 số
                FontSize = 14
            };
            stack.Children.Add(textBox);

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var btnOK = new Button
            {
                Content = "OK",
                Width = 80,
                Margin = new Thickness(0, 0, 10, 0),
                // SỬA LỖI TẠI ĐÂY: Dùng 4 tham số (Left, Top, Right, Bottom)
                Padding = new Thickness(10, 5, 10, 5),
                IsDefault = true
            };
            btnOK.Click += (s, e) => { dialog.DialogResult = true; dialog.Close(); };

            var btnCancel = new Button
            {
                Content = "Hủy",
                Width = 80,
                // SỬA LỖI TẠI ĐÂY: Dùng 4 tham số
                Padding = new Thickness(10, 5, 10, 5),
                IsCancel = true
            };
            btnCancel.Click += (s, e) => { dialog.DialogResult = false; dialog.Close(); };

            btnPanel.Children.Add(btnOK);
            btnPanel.Children.Add(btnCancel);
            stack.Children.Add(btnPanel);

            dialog.Content = stack;
            textBox.Focus();

            return dialog.ShowDialog() == true ? textBox.Text : null;
        }

        private bool ConfirmDelete(string itemName)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa {itemName} này?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}