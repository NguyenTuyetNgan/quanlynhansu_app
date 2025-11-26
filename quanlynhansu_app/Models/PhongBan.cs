using System;
using System.ComponentModel;

namespace quanlynhansu_app.Models
{
    /// <summary>
    /// Model Phòng ban
    /// </summary>
    public class PhongBan : INotifyPropertyChanged
    {
        private int id;
        private string maPhongBan;
        private string tenPhongBan;
        private int? truongPhong;
        private string moTa;
        private string tenTruongPhong;
        private int soNhanVien;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string MaPhongBan
        {
            get => maPhongBan;
            set { maPhongBan = value; OnPropertyChanged(nameof(MaPhongBan)); }
        }

        public string TenPhongBan
        {
            get => tenPhongBan;
            set { tenPhongBan = value; OnPropertyChanged(nameof(TenPhongBan)); }
        }

        public int? TruongPhong
        {
            get => truongPhong;
            set { truongPhong = value; OnPropertyChanged(nameof(TruongPhong)); }
        }

        public string MoTa
        {
            get => moTa;
            set { moTa = value; OnPropertyChanged(nameof(MoTa)); }
        }

        public string TenTruongPhong
        {
            get => tenTruongPhong;
            set { tenTruongPhong = value; OnPropertyChanged(nameof(TenTruongPhong)); }
        }

        public int SoNhanVien
        {
            get => soNhanVien;
            set { soNhanVien = value; OnPropertyChanged(nameof(SoNhanVien)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Model Chức vụ
    /// </summary>
    public class ChucVu
    {
        public int Id { get; set; }
        public string TenChucVu { get; set; }
    }

    /// <summary>
    /// Model Loại hợp đồng
    /// </summary>
    public class LoaiHopDong
    {
        public int Id { get; set; }
        public string TenLoai { get; set; }
    }

    /// <summary>
    /// Model Trình độ học vấn
    /// </summary>
    public class TrinhDoHocVan
    {
        public int Id { get; set; }
        public string TenTrinhDo { get; set; }
    }

    /// <summary>
    /// Model Trạng thái nhân viên
    /// </summary>
    public class TrangThaiNhanVien
    {
        public int Id { get; set; }
        public string TenTrangThai { get; set; }
    }

    /// <summary>
    /// Model Loại tài liệu
    /// </summary>
    public class LoaiTaiLieu
    {
        public int Id { get; set; }
        public string TenLoai { get; set; }
    }

    /// <summary>
    /// Model Tài liệu nhân sự
    /// </summary>
    public class TaiLieuNhanSu
    {
        public int Id { get; set; }
        public int NhanSuId { get; set; }
        public int LoaiTaiLieuId { get; set; }
        public string TenTaiLieu { get; set; }
        public string DuongDanFile { get; set; }
        public string GhiChu { get; set; }
        public DateTime CreatedAt { get; set; }

        // Join
        public string TenLoai { get; set; }
    }

    /// <summary>
    /// Model User cho đăng nhập
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "admin" hoặc "user"
        public int? NhanSuId { get; set; }
    }

    /// <summary>
    /// Model cho thống kê Dashboard
    /// </summary>
    public class ThongKe
    {
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public double PhanTram { get; set; }
    }
}