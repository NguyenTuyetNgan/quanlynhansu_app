using MySql.Data.MySqlClient;
using quanlynhansu_app.Data;
using quanlynhansu_app.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace quanlynhansu_app.Services
{
    /// <summary>
    /// Service quản lý các thao tác nghiệp vụ với Nhân sự
    /// </summary>
    public class NhanSuService
    {
        /// <summary>
        /// Lấy danh sách tất cả nhân sự (có join với các bảng liên quan)
        /// </summary>
        public List<NhanSu> GetAllNhanSu()
        {
            List<NhanSu> list = new List<NhanSu>();

            string query = @"
                SELECT ns.*, 
                       pb.ten_phong_ban,
                       cv.ten_chuc_vu,
                       tt.ten_trang_thai,
                       lhd.ten_loai as loai_hop_dong,
                       td.ten_trinh_do
                FROM nhan_su ns
                LEFT JOIN phong_ban pb ON ns.phong_ban_id = pb.id
                LEFT JOIN chuc_vu cv ON ns.chuc_vu_id = cv.id
                LEFT JOIN trang_thai_nhan_vien tt ON ns.trang_thai_id = tt.id
                LEFT JOIN loai_hop_dong lhd ON ns.loai_hop_dong_id = lhd.id
                LEFT JOIN trinh_do_hoc_van td ON ns.trinh_do_hoc_van_id = td.id
                ORDER BY ns.created_at DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToNhanSu(row));
            }

            return list;
        }

        /// <summary>
        /// Lấy nhân sự theo ID
        /// </summary>
        public NhanSu GetNhanSuById(int id)
        {
            string query = @"
                SELECT ns.*, 
                       pb.ten_phong_ban,
                       cv.ten_chuc_vu,
                       tt.ten_trang_thai,
                       lhd.ten_loai as loai_hop_dong,
                       td.ten_trinh_do
                FROM nhan_su ns
                LEFT JOIN phong_ban pb ON ns.phong_ban_id = pb.id
                LEFT JOIN chuc_vu cv ON ns.chuc_vu_id = cv.id
                LEFT JOIN trang_thai_nhan_vien tt ON ns.trang_thai_id = tt.id
                LEFT JOIN loai_hop_dong lhd ON ns.loai_hop_dong_id = lhd.id
                LEFT JOIN trinh_do_hoc_van td ON ns.trinh_do_hoc_van_id = td.id
                WHERE ns.id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToNhanSu(dt.Rows[0]);
            }

            return null;
        }

        /// <summary>
        /// Tìm kiếm nhân sự theo từ khóa
        /// </summary>
        public List<NhanSu> SearchNhanSu(string keyword)
        {
            List<NhanSu> list = new List<NhanSu>();

            string query = @"
                SELECT ns.*, 
                       pb.ten_phong_ban,
                       cv.ten_chuc_vu,
                       tt.ten_trang_thai,
                       lhd.ten_loai as loai_hop_dong,
                       td.ten_trinh_do
                FROM nhan_su ns
                LEFT JOIN phong_ban pb ON ns.phong_ban_id = pb.id
                LEFT JOIN chuc_vu cv ON ns.chuc_vu_id = cv.id
                LEFT JOIN trang_thai_nhan_vien tt ON ns.trang_thai_id = tt.id
                LEFT JOIN loai_hop_dong lhd ON ns.loai_hop_dong_id = lhd.id
                LEFT JOIN trinh_do_hoc_van td ON ns.trinh_do_hoc_van_id = td.id
                WHERE ns.ma_nhan_vien LIKE @Keyword 
                   OR ns.ho_ten LIKE @Keyword
                   OR ns.email LIKE @Keyword
                ORDER BY ns.created_at DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Keyword", "%" + keyword + "%")
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToNhanSu(row));
            }

            return list;
        }

        /// <summary>
        /// Thêm nhân sự mới
        /// </summary>
        public bool AddNhanSu(NhanSu nhanSu)
        {
            string query = @"
                INSERT INTO nhan_su 
                (ma_nhan_vien, ho_ten, ngay_sinh, gioi_tinh, so_dien_thoai, 
                 email, dia_chi, anh_dai_dien, chuc_vu_id, phong_ban_id, 
                 ngay_vao_lam, ngay_nghi_viec, loai_hop_dong_id, muc_luong, 
                 trinh_do_hoc_van_id, trang_thai_id)
                VALUES 
                (@MaNhanVien, @HoTen, @NgaySinh, @GioiTinh, @SoDienThoai,
                 @Email, @DiaChi, @AnhDaiDien, @ChucVuId, @PhongBanId,
                 @NgayVaoLam, @NgayNghiViec, @LoaiHopDongId, @MucLuong,
                 @TrinhDoHocVanId, @TrangThaiId)";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@MaNhanVien", nhanSu.MaNhanVien),
                new MySqlParameter("@HoTen", nhanSu.HoTen),
                new MySqlParameter("@NgaySinh", (object)nhanSu.NgaySinh ?? DBNull.Value),
                new MySqlParameter("@GioiTinh", nhanSu.GioiTinh ?? "Nam"),
                new MySqlParameter("@SoDienThoai", (object)nhanSu.SoDienThoai ?? DBNull.Value),
                new MySqlParameter("@Email", (object)nhanSu.Email ?? DBNull.Value),
                new MySqlParameter("@DiaChi", (object)nhanSu.DiaChi ?? DBNull.Value),
                new MySqlParameter("@AnhDaiDien", (object)nhanSu.AnhDaiDien ?? DBNull.Value),
                new MySqlParameter("@ChucVuId", (object)nhanSu.ChucVuId ?? DBNull.Value),
                new MySqlParameter("@PhongBanId", (object)nhanSu.PhongBanId ?? DBNull.Value),
                new MySqlParameter("@NgayVaoLam", (object)nhanSu.NgayVaoLam ?? DBNull.Value),
                new MySqlParameter("@NgayNghiViec", (object)nhanSu.NgayNghiViec ?? DBNull.Value),
                new MySqlParameter("@LoaiHopDongId", (object)nhanSu.LoaiHopDongId ?? DBNull.Value),
                new MySqlParameter("@MucLuong", (object)nhanSu.MucLuong ?? DBNull.Value),
                new MySqlParameter("@TrinhDoHocVanId", (object)nhanSu.TrinhDoHocVanId ?? DBNull.Value),
                new MySqlParameter("@TrangThaiId", nhanSu.TrangThaiId)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        /// <summary>
        /// Cập nhật thông tin nhân sự
        /// </summary>
        public bool UpdateNhanSu(NhanSu nhanSu)
        {
            string query = @"
                UPDATE nhan_su SET
                    ma_nhan_vien = @MaNhanVien,
                    ho_ten = @HoTen,
                    ngay_sinh = @NgaySinh,
                    gioi_tinh = @GioiTinh,
                    so_dien_thoai = @SoDienThoai,
                    email = @Email,
                    dia_chi = @DiaChi,
                    anh_dai_dien = @AnhDaiDien,
                    chuc_vu_id = @ChucVuId,
                    phong_ban_id = @PhongBanId,
                    ngay_vao_lam = @NgayVaoLam,
                    ngay_nghi_viec = @NgayNghiViec,
                    loai_hop_dong_id = @LoaiHopDongId,
                    muc_luong = @MucLuong,
                    trinh_do_hoc_van_id = @TrinhDoHocVanId,
                    trang_thai_id = @TrangThaiId
                WHERE id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", nhanSu.Id),
                new MySqlParameter("@MaNhanVien", nhanSu.MaNhanVien),
                new MySqlParameter("@HoTen", nhanSu.HoTen),
                new MySqlParameter("@NgaySinh", (object)nhanSu.NgaySinh ?? DBNull.Value),
                new MySqlParameter("@GioiTinh", nhanSu.GioiTinh ?? "Nam"),
                new MySqlParameter("@SoDienThoai", (object)nhanSu.SoDienThoai ?? DBNull.Value),
                new MySqlParameter("@Email", (object)nhanSu.Email ?? DBNull.Value),
                new MySqlParameter("@DiaChi", (object)nhanSu.DiaChi ?? DBNull.Value),
                new MySqlParameter("@AnhDaiDien", (object)nhanSu.AnhDaiDien ?? DBNull.Value),
                new MySqlParameter("@ChucVuId", (object)nhanSu.ChucVuId ?? DBNull.Value),
                new MySqlParameter("@PhongBanId", (object)nhanSu.PhongBanId ?? DBNull.Value),
                new MySqlParameter("@NgayVaoLam", (object)nhanSu.NgayVaoLam ?? DBNull.Value),
                new MySqlParameter("@NgayNghiViec", (object)nhanSu.NgayNghiViec ?? DBNull.Value),
                new MySqlParameter("@LoaiHopDongId", (object)nhanSu.LoaiHopDongId ?? DBNull.Value),
                new MySqlParameter("@MucLuong", (object)nhanSu.MucLuong ?? DBNull.Value),
                new MySqlParameter("@TrinhDoHocVanId", (object)nhanSu.TrinhDoHocVanId ?? DBNull.Value),
                new MySqlParameter("@TrangThaiId", nhanSu.TrangThaiId)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        /// <summary>
        /// Xóa nhân sự
        /// </summary>
        public bool DeleteNhanSu(int id)
        {
            string query = "DELETE FROM nhan_su WHERE id = @Id";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        /// <summary>
        /// Hàm map từ DataRow sang object NhanSu
        /// </summary>
        private NhanSu MapDataRowToNhanSu(DataRow row)
        {
            return new NhanSu
            {
                Id = Convert.ToInt32(row["id"]),
                MaNhanVien = row["ma_nhan_vien"].ToString(),
                HoTen = row["ho_ten"].ToString(),
                NgaySinh = row["ngay_sinh"] != DBNull.Value ? Convert.ToDateTime(row["ngay_sinh"]) : null,
                GioiTinh = row["gioi_tinh"].ToString(),
                SoDienThoai = row["so_dien_thoai"]?.ToString(),
                Email = row["email"]?.ToString(),
                DiaChi = row["dia_chi"]?.ToString(),
                AnhDaiDien = row["anh_dai_dien"]?.ToString(),
                ChucVuId = row["chuc_vu_id"] != DBNull.Value ? Convert.ToInt32(row["chuc_vu_id"]) : null,
                PhongBanId = row["phong_ban_id"] != DBNull.Value ? Convert.ToInt32(row["phong_ban_id"]) : null,
                NgayVaoLam = row["ngay_vao_lam"] != DBNull.Value ? Convert.ToDateTime(row["ngay_vao_lam"]) : null,
                NgayNghiViec = row["ngay_nghi_viec"] != DBNull.Value ? Convert.ToDateTime(row["ngay_nghi_viec"]) : null,
                LoaiHopDongId = row["loai_hop_dong_id"] != DBNull.Value ? Convert.ToInt32(row["loai_hop_dong_id"]) : null,
                MucLuong = row["muc_luong"] != DBNull.Value ? Convert.ToDecimal(row["muc_luong"]) : null,
                TrinhDoHocVanId = row["trinh_do_hoc_van_id"] != DBNull.Value ? Convert.ToInt32(row["trinh_do_hoc_van_id"]) : null,
                TrangThaiId = Convert.ToInt32(row["trang_thai_id"]),

                // Join fields
                TenPhongBan = row["ten_phong_ban"]?.ToString(),
                TenChucVu = row["ten_chuc_vu"]?.ToString(),
                TenTrangThai = row["ten_trang_thai"]?.ToString(),
                LoaiHopDong = row["loai_hop_dong"]?.ToString(),
                TrinhDoHocVan = row["ten_trinh_do"]?.ToString()
            };
        }
    }
}