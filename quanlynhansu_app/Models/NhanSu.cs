using System;
using System.ComponentModel;

namespace quanlynhansu_app.Models
{
    /// <summary>
    /// Model đại diện cho 1 nhân viên trong hệ thống
    /// Sử dụng INotifyPropertyChanged để binding dữ liệu với UI
    /// </summary>
    public class NhanSu : INotifyPropertyChanged
    {
        private int id;
        private string maNhanVien;
        private string hoTen;
        private DateTime? ngaySinh;
        private string gioiTinh;
        private string soDienThoai;
        private string email;
        private string diaChi;
        private string anhDaiDien;

        // Foreign keys
        private int? chucVuId;
        private int? phongBanId;
        private DateTime? ngayVaoLam;
        private DateTime? ngayNghiViec;
        private int? loaiHopDongId;
        private decimal? mucLuong;
        private int? trinhDoHocVanId;
        private int trangThaiId;

        // Các thuộc tính join từ bảng khác
        private string tenChucVu;
        private string tenPhongBan;
        private string tenTrangThai;
        private string loaiHopDong;
        private string trinhDoHocVan;

        // Properties với INotifyPropertyChanged
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string MaNhanVien
        {
            get => maNhanVien;
            set { maNhanVien = value; OnPropertyChanged(nameof(MaNhanVien)); }
        }

        public string HoTen
        {
            get => hoTen;
            set { hoTen = value; OnPropertyChanged(nameof(HoTen)); }
        }

        public DateTime? NgaySinh
        {
            get => ngaySinh;
            set { ngaySinh = value; OnPropertyChanged(nameof(NgaySinh)); }
        }

        public string GioiTinh
        {
            get => gioiTinh;
            set { gioiTinh = value; OnPropertyChanged(nameof(GioiTinh)); }
        }

        public string SoDienThoai
        {
            get => soDienThoai;
            set { soDienThoai = value; OnPropertyChanged(nameof(SoDienThoai)); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string DiaChi
        {
            get => diaChi;
            set { diaChi = value; OnPropertyChanged(nameof(DiaChi)); }
        }

        public string AnhDaiDien
        {
            get => anhDaiDien;
            set { anhDaiDien = value; OnPropertyChanged(nameof(AnhDaiDien)); }
        }

        public int? ChucVuId
        {
            get => chucVuId;
            set { chucVuId = value; OnPropertyChanged(nameof(ChucVuId)); }
        }

        public int? PhongBanId
        {
            get => phongBanId;
            set { phongBanId = value; OnPropertyChanged(nameof(PhongBanId)); }
        }

        public DateTime? NgayVaoLam
        {
            get => ngayVaoLam;
            set { ngayVaoLam = value; OnPropertyChanged(nameof(NgayVaoLam)); }
        }

        public DateTime? NgayNghiViec
        {
            get => ngayNghiViec;
            set { ngayNghiViec = value; OnPropertyChanged(nameof(NgayNghiViec)); }
        }

        public int? LoaiHopDongId
        {
            get => loaiHopDongId;
            set { loaiHopDongId = value; OnPropertyChanged(nameof(LoaiHopDongId)); }
        }

        public decimal? MucLuong
        {
            get => mucLuong;
            set { mucLuong = value; OnPropertyChanged(nameof(MucLuong)); }
        }

        public int? TrinhDoHocVanId
        {
            get => trinhDoHocVanId;
            set { trinhDoHocVanId = value; OnPropertyChanged(nameof(TrinhDoHocVanId)); }
        }

        public int TrangThaiId
        {
            get => trangThaiId;
            set { trangThaiId = value; OnPropertyChanged(nameof(TrangThaiId)); }
        }

        // Thuộc tính join
        public string TenChucVu
        {
            get => tenChucVu;
            set { tenChucVu = value; OnPropertyChanged(nameof(TenChucVu)); }
        }

        public string TenPhongBan
        {
            get => tenPhongBan;
            set { tenPhongBan = value; OnPropertyChanged(nameof(TenPhongBan)); }
        }

        public string TenTrangThai
        {
            get => tenTrangThai;
            set { tenTrangThai = value; OnPropertyChanged(nameof(TenTrangThai)); }
        }

        public string LoaiHopDong
        {
            get => loaiHopDong;
            set { loaiHopDong = value; OnPropertyChanged(nameof(LoaiHopDong)); }
        }

        public string TrinhDoHocVan
        {
            get => trinhDoHocVan;
            set { trinhDoHocVan = value; OnPropertyChanged(nameof(TrinhDoHocVan)); }
        }

        // Thuộc tính computed để hiển thị lương format
        public string MucLuongFormatted => MucLuong.HasValue ?
            string.Format("{0:N0} VNĐ", MucLuong.Value) : "-";

        // Event cho INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}