using MySql.Data.MySqlClient;
using quanlynhansu_app.Data;
using quanlynhansu_app.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace quanlynhansu_app.Services
{
    /// <summary>
    /// Service quản lý các thao tác nghiệp vụ với Phòng ban
    /// </summary>
    public class PhongBanService
    {
        /// <summary>
        /// Lấy danh sách tất cả phòng ban (có join với nhân sự để lấy tên trưởng phòng)
        /// </summary>
        public List<PhongBan> GetAllPhongBan()
        {
            List<PhongBan> list = new List<PhongBan>();

            string query = @"
                SELECT pb.*, 
                       ns.ho_ten as ten_truong_phong,
                       ns.ma_nhan_vien as ma_truong_phong,
                       (SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = pb.id) as so_nhan_vien
                FROM phong_ban pb
                LEFT JOIN nhan_su ns ON pb.truong_phong = ns.id
                ORDER BY pb.created_at DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToPhongBan(row));
            }

            return list;
        }

        /// <summary>
        /// Lấy phòng ban theo ID
        /// </summary>
        public PhongBan GetPhongBanById(int id)
        {
            string query = @"
                SELECT pb.*, 
                       ns.ho_ten as ten_truong_phong,
                       ns.ma_nhan_vien as ma_truong_phong,
                       (SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = pb.id) as so_nhan_vien
                FROM phong_ban pb
                LEFT JOIN nhan_su ns ON pb.truong_phong = ns.id
                WHERE pb.id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToPhongBan(dt.Rows[0]);
            }

            return null;
        }

        /// <summary>
        /// Tìm kiếm phòng ban theo từ khóa
        /// </summary>
        public List<PhongBan> SearchPhongBan(string keyword)
        {
            List<PhongBan> list = new List<PhongBan>();

            string query = @"
                SELECT pb.*, 
                       ns.ho_ten as ten_truong_phong,
                       ns.ma_nhan_vien as ma_truong_phong,
                       (SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = pb.id) as so_nhan_vien
                FROM phong_ban pb
                LEFT JOIN nhan_su ns ON pb.truong_phong = ns.id
                WHERE pb.ma_phong_ban LIKE @Keyword 
                   OR pb.ten_phong_ban LIKE @Keyword
                ORDER BY pb.created_at DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Keyword", "%" + keyword + "%")
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToPhongBan(row));
            }

            return list;
        }

        /// <summary>
        /// Thêm phòng ban mới
        /// </summary>
        public bool AddPhongBan(PhongBan phongBan)
        {
            string query = @"
                INSERT INTO phong_ban 
                (ma_phong_ban, ten_phong_ban, truong_phong, mo_ta)
                VALUES 
                (@MaPhongBan, @TenPhongBan, @TruongPhong, @MoTa)";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@MaPhongBan", phongBan.MaPhongBan),
                new MySqlParameter("@TenPhongBan", phongBan.TenPhongBan),
                new MySqlParameter("@TruongPhong", (object)phongBan.TruongPhong ?? DBNull.Value),
                new MySqlParameter("@MoTa", (object)phongBan.MoTa ?? DBNull.Value)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        /// <summary>
        /// Cập nhật thông tin phòng ban
        /// </summary>
        public bool UpdatePhongBan(PhongBan phongBan)
        {
            string query = @"
                UPDATE phong_ban SET
                    ma_phong_ban = @MaPhongBan,
                    ten_phong_ban = @TenPhongBan,
                    truong_phong = @TruongPhong,
                    mo_ta = @MoTa
                WHERE id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", phongBan.Id),
                new MySqlParameter("@MaPhongBan", phongBan.MaPhongBan),
                new MySqlParameter("@TenPhongBan", phongBan.TenPhongBan),
                new MySqlParameter("@TruongPhong", (object)phongBan.TruongPhong ?? DBNull.Value),
                new MySqlParameter("@MoTa", (object)phongBan.MoTa ?? DBNull.Value)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        /// <summary>
        /// Xóa phòng ban
        /// Lưu ý: Chỉ xóa được nếu phòng ban không có nhân viên
        /// </summary>
        public bool DeletePhongBan(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Kiểm tra xem phòng ban có nhân viên không
                string checkQuery = "SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = @Id";
                var checkParams = new MySqlParameter[] { new MySqlParameter("@Id", id) };

                object countObj = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);
                int count = Convert.ToInt32(countObj);

                if (count > 0)
                {
                    errorMessage = $"Không thể xóa phòng ban này vì còn {count} nhân viên!";
                    return false;
                }

                // Xóa phòng ban
                string deleteQuery = "DELETE FROM phong_ban WHERE id = @Id";
                var deleteParams = new MySqlParameter[] { new MySqlParameter("@Id", id) };

                int result = DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteParams);
                return result > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên trong phòng ban
        /// </summary>
        public List<NhanSu> GetNhanSuByPhongBan(int phongBanId)
        {
            List<NhanSu> list = new List<NhanSu>();

            string query = @"
                SELECT ns.*, 
                       pb.ten_phong_ban,
                       cv.ten_chuc_vu,
                       tt.ten_trang_thai
                FROM nhan_su ns
                LEFT JOIN phong_ban pb ON ns.phong_ban_id = pb.id
                LEFT JOIN chuc_vu cv ON ns.chuc_vu_id = cv.id
                LEFT JOIN trang_thai_nhan_vien tt ON ns.trang_thai_id = tt.id
                WHERE ns.phong_ban_id = @PhongBanId
                ORDER BY ns.ho_ten";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@PhongBanId", phongBanId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToNhanSu(row));
            }

            return list;
        }

        /// <summary>
        /// Lấy thống kê phòng ban
        /// </summary>
        public Dictionary<string, int> GetPhongBanStatistics(int phongBanId)
        {
            var stats = new Dictionary<string, int>();

            try
            {
                // Tổng số nhân viên
                string queryTotal = "SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = @Id";
                var total = DatabaseHelper.ExecuteScalar(queryTotal, new MySqlParameter("@Id", phongBanId));
                stats["TongNhanVien"] = Convert.ToInt32(total);

                // Đang làm việc
                string queryActive = "SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = @Id AND trang_thai_id = 1";
                var active = DatabaseHelper.ExecuteScalar(queryActive, new MySqlParameter("@Id", phongBanId));
                stats["DangLamViec"] = Convert.ToInt32(active);

                // Nghỉ sinh
                string queryMaternity = "SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = @Id AND trang_thai_id = 2";
                var maternity = DatabaseHelper.ExecuteScalar(queryMaternity, new MySqlParameter("@Id", phongBanId));
                stats["NghiSinh"] = Convert.ToInt32(maternity);

                // Đã nghỉ
                string queryResigned = "SELECT COUNT(*) FROM nhan_su WHERE phong_ban_id = @Id AND trang_thai_id = 3";
                var resigned = DatabaseHelper.ExecuteScalar(queryResigned, new MySqlParameter("@Id", phongBanId));
                stats["DaNghi"] = Convert.ToInt32(resigned);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lấy thống kê: {ex.Message}");
            }

            return stats;
        }

        /// <summary>
        /// Hàm map từ DataRow sang object PhongBan
        /// </summary>
        private PhongBan MapDataRowToPhongBan(DataRow row)
        {
            return new PhongBan
            {
                Id = Convert.ToInt32(row["id"]),
                MaPhongBan = row["ma_phong_ban"].ToString(),
                TenPhongBan = row["ten_phong_ban"].ToString(),
                TruongPhong = row["truong_phong"] != DBNull.Value ? Convert.ToInt32(row["truong_phong"]) : null,
                MoTa = row["mo_ta"]?.ToString(),
                TenTruongPhong = row["ten_truong_phong"]?.ToString(),
                SoNhanVien = row["so_nhan_vien"] != DBNull.Value ? Convert.ToInt32(row["so_nhan_vien"]) : 0
            };
        }

        /// <summary>
        /// Hàm map từ DataRow sang object NhanSu (dùng cho GetNhanSuByPhongBan)
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
                PhongBanId = row["phong_ban_id"] != DBNull.Value ? Convert.ToInt32(row["phong_ban_id"]) : null,
                ChucVuId = row["chuc_vu_id"] != DBNull.Value ? Convert.ToInt32(row["chuc_vu_id"]) : null,
                TrangThaiId = Convert.ToInt32(row["trang_thai_id"]),
                TenPhongBan = row["ten_phong_ban"]?.ToString(),
                TenChucVu = row["ten_chuc_vu"]?.ToString(),
                TenTrangThai = row["ten_trang_thai"]?.ToString()
            };
        }
    }
}