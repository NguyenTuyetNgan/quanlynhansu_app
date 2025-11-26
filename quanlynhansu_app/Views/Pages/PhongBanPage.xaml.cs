using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using quanlynhansu_app.Models;
using quanlynhansu_app.Services;

namespace quanlynhansu_app.Views.Pages
{
    /// <summary>
    /// Trang quản lý phòng ban - Hiển thị dạng card grid như web app
    /// </summary>
    public partial class PhongBanPage : Page
    {
        private PhongBanService phongBanService;
        private List<PhongBan> allPhongBan;

        public PhongBanPage()
        {
            InitializeComponent();
            phongBanService = new PhongBanService();
            LoadData();
        }

        /// <summary>
        /// Load dữ liệu phòng ban và hiển thị dạng card
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Lấy danh sách phòng ban
                allPhongBan = phongBanService.GetAllPhongBan();

                // Hiển thị lên grid
                DisplayPhongBanCards(allPhongBan);
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
        /// Hiển thị danh sách phòng ban dạng card grid
        /// Tương tự giao diện web với layout responsive
        /// </summary>
        private void DisplayPhongBanCards(List<PhongBan> phongBanList)
        {
            gridPhongBan.Children.Clear();
            gridPhongBan.RowDefinitions.Clear();
            gridPhongBan.ColumnDefinitions.Clear();

            // Tạo grid 3 cột (responsive)
            int columns = 3;
            for (int i = 0; i < columns; i++)
            {
                gridPhongBan.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Tạo các card
            int row = 0;
            int col = 0;

            foreach (var phongBan in phongBanList)
            {
                // Tạo row mới nếu cần
                if (col == 0)
                {
                    gridPhongBan.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                // Tạo card
                var card = CreatePhongBanCard(phongBan);
                Grid.SetRow(card, row);
                Grid.SetColumn(card, col);

                // Set margin cho card
                if (col < columns - 1)
                    card.Margin = new Thickness(0, 0, 10, 20);
                else
                    card.Margin = new Thickness(0, 0, 0, 20);

                gridPhongBan.Children.Add(card);

                // Tăng column
                col++;
                if (col >= columns)
                {
                    col = 0;
                    row++;
                }
            }

            // Thêm card "Thêm phòng ban mới" (nếu có chỗ)
            if (col > 0) // Chỉ hiện nếu hàng hiện tại vẫn còn chỗ trống hoặc muốn xuống dòng
            {            // Logic này tùy bạn, thường thì nút Add luôn hiện ở cuối

                // (Đoạn này mình chỉnh lại logic một chút để nút Add luôn hiện kể cả khi xuống dòng mới)
            }

            // Logic thêm nút Add: Luôn thêm vào vị trí tiếp theo
            if (col == 0)
            {
                gridPhongBan.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            var addCard = CreateAddCard();
            Grid.SetRow(addCard, row);
            Grid.SetColumn(addCard, col);

            if (col < columns - 1)
                addCard.Margin = new Thickness(0, 0, 10, 20);
            else
                addCard.Margin = new Thickness(0, 0, 0, 20);

            gridPhongBan.Children.Add(addCard);
        }

        /// <summary>
        /// Tạo card cho mỗi phòng ban (giống web app)
        /// </summary>
        private Border CreatePhongBanCard(PhongBan phongBan)
        {
            // Container card
            var card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(25),
                Tag = phongBan,
                Cursor = Cursors.Hand
            };

            // Drop shadow effect
            card.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                Color = Colors.Black,
                Opacity = 0.05,
                BlurRadius = 10,
                ShadowDepth = 2
            };

            // Hover effect
            card.MouseEnter += (s, e) =>
            {
                card.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    Opacity = 0.15,
                    BlurRadius = 20,
                    ShadowDepth = 5
                };
            };

            card.MouseLeave += (s, e) =>
            {
                card.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    Opacity = 0.05,
                    BlurRadius = 10,
                    ShadowDepth = 2
                };
            };

            // Content stack
            var stack = new StackPanel();

            // Icon phòng ban
            var iconBorder = new Border
            {
                Width = 60,
                Height = 60,
                CornerRadius = new CornerRadius(12),
                Background = new SolidColorBrush(Color.FromRgb(102, 126, 234)),
                Margin = new Thickness(0, 0, 0, 15)
            };

            var iconText = new TextBlock
            {
                Text = "🏢",
                FontSize = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            iconBorder.Child = iconText;
            stack.Children.Add(iconBorder);

            // Mã phòng ban
            var maPhongBan = new TextBlock
            {
                Text = phongBan.MaPhongBan,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 126, 234)),
                Background = new SolidColorBrush(Color.FromRgb(240, 244, 255)),
                Padding = new Thickness(10, 4, 10, 4),
                Margin = new Thickness(0, 0, 0, 8),
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            stack.Children.Add(maPhongBan);

            // Tên phòng ban
            var tenPhongBan = new TextBlock
            {
                Text = phongBan.TenPhongBan,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                Margin = new Thickness(0, 0, 0, 15),
                TextWrapping = TextWrapping.Wrap
            };
            stack.Children.Add(tenPhongBan);

            // Thông tin
            var infoStack = new StackPanel { Margin = new Thickness(0, 0, 0, 15) };

            // Trưởng phòng
            var truongPhongPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            truongPhongPanel.Children.Add(new TextBlock
            {
                Text = "👤 Trưởng phòng:",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 0, 10, 0)
            });
            truongPhongPanel.Children.Add(new TextBlock
            {
                Text = string.IsNullOrEmpty(phongBan.TenTruongPhong) ? "Chưa có" : phongBan.TenTruongPhong,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51))
            });
            infoStack.Children.Add(truongPhongPanel);

            // Số nhân viên
            var nhanVienPanel = new StackPanel { Orientation = Orientation.Horizontal };
            nhanVienPanel.Children.Add(new TextBlock
            {
                Text = "👥 Số nhân viên:",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 0, 10, 0)
            });
            nhanVienPanel.Children.Add(new TextBlock
            {
                Text = $"{phongBan.SoNhanVien} người",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 126, 234))
            });
            infoStack.Children.Add(nhanVienPanel);

            stack.Children.Add(infoStack);

            // Mô tả (nếu có)
            if (!string.IsNullOrEmpty(phongBan.MoTa))
            {
                var moTa = new TextBlock
                {
                    Text = $"📝 {phongBan.MoTa}",
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 15)
                };
                stack.Children.Add(moTa);
            }

            // Buttons
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var btnEdit = new Button
            {
                Content = "✏️",
                // Chỗ này nếu project bạn không có Style này thì xóa dòng Style đi
                // Style = Application.Current.Resources["MaterialDesignIconButton"] as Style, 
                ToolTip = "Chỉnh sửa",
                Tag = phongBan.Id,
                Width = 40,
                Height = 40,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };
            btnEdit.Click += BtnEdit_Click;
            buttonPanel.Children.Add(btnEdit);

            var btnDelete = new Button
            {
                Content = "🗑️",
                // Style = Application.Current.Resources["MaterialDesignIconButton"] as Style,
                ToolTip = "Xóa",
                Tag = phongBan.Id,
                Margin = new Thickness(5, 0, 0, 0),
                Width = 40,
                Height = 40,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };
            btnDelete.Click += BtnDelete_Click;
            buttonPanel.Children.Add(btnDelete);

            stack.Children.Add(buttonPanel);

            card.Child = stack;
            return card;
        }

        /// <summary>
        /// Tạo card "Thêm phòng ban mới" (Sửa lỗi BorderDashArray)
        /// CHÚ Ý: Đã đổi kiểu trả về thành FrameworkElement để dùng được Margin
        /// </summary>
        private FrameworkElement CreateAddCard()
        {
            // 1. Tạo Grid chứa (thay vì Border) để có thể xếp chồng Rectangle và Nội dung
            var containerGrid = new Grid
            {
                MinHeight = 280,
                Cursor = Cursors.Hand,
                Background = Brushes.Transparent // Để bắt sự kiện click chuột
            };

            // 2. Tạo hình chữ nhật có viền nét đứt (Dashed Border)
            var dashedRect = new System.Windows.Shapes.Rectangle
            {
                Stroke = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                StrokeThickness = 2,
                RadiusX = 12, // Bo góc
                RadiusY = 12,
                Fill = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                StrokeDashArray = new DoubleCollection { 5, 3 }
            };

            // Thêm hình chữ nhật vào Grid
            containerGrid.Children.Add(dashedRect);

            // 3. Tạo nội dung bên trong (StackPanel)
            var stack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var icon = new TextBlock
            {
                Text = "➕",
                FontSize = 48,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 126, 234)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stack.Children.Add(icon);

            var text = new TextBlock
            {
                Text = "Thêm phòng ban mới",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 126, 234)),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stack.Children.Add(text);

            // Thêm nội dung đè lên trên hình chữ nhật
            containerGrid.Children.Add(stack);

            // 4. Xử lý sự kiện Click
            containerGrid.MouseLeftButtonDown += (s, e) => BtnAdd_Click(s, e);

            // 5. Hiệu ứng Hover (Thay đổi màu của Rectangle bên trong)
            containerGrid.MouseEnter += (s, e) =>
            {
                dashedRect.Fill = new SolidColorBrush(Color.FromRgb(240, 244, 255));
                dashedRect.Stroke = new SolidColorBrush(Color.FromRgb(102, 126, 234));
            };

            containerGrid.MouseLeave += (s, e) =>
            {
                dashedRect.Fill = new SolidColorBrush(Color.FromRgb(250, 250, 250));
                dashedRect.Stroke = new SolidColorBrush(Color.FromRgb(224, 224, 224));
            };

            return containerGrid;
        }

        /// <summary>
        /// Tìm kiếm khi gõ
        /// </summary>
        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                DisplayPhongBanCards(allPhongBan);
            }
            else
            {
                var filtered = allPhongBan.Where(pb =>
                    pb.MaPhongBan.ToLower().Contains(keyword) ||
                    pb.TenPhongBan.ToLower().Contains(keyword)
                ).ToList();

                DisplayPhongBanCards(filtered);
            }
        }

        /// <summary>
        /// Reset filter
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            DisplayPhongBanCards(allPhongBan);
        }

        /// <summary>
        /// Thêm phòng ban
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Đảm bảo bạn có file Dialogs/PhongBanDialog.xaml
            var dialog = new Dialogs.PhongBanDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        /// <summary>
        /// Chỉnh sửa phòng ban
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var phongBan = phongBanService.GetPhongBanById(id);
                if (phongBan != null)
                {
                    var dialog = new Dialogs.PhongBanDialog(phongBan);
                    if (dialog.ShowDialog() == true)
                    {
                        LoadData();
                    }
                }
            }
        }

        /// <summary>
        /// Xóa phòng ban
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var phongBan = phongBanService.GetPhongBanById(id);
                if (phongBan != null)
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc muốn xóa phòng ban '{phongBan.TenPhongBan}'?\n\n" +
                        $"Hiện có {phongBan.SoNhanVien} nhân viên trong phòng ban này.",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            bool success = phongBanService.DeletePhongBan(id, out string errorMessage);

                            if (success)
                            {
                                MessageBox.Show(
                                    "Xóa phòng ban thành công!",
                                    "Thành công",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show(
                                    errorMessage,
                                    "Không thể xóa",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
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
        }
    }
}