using ExcelDataReader;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using quanlynhansu_app.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace quanlynhansu_app.Services
{
    public class ExcelService
    {
        public ExcelService()
        {
            // Đăng ký Encoding để đọc file Excel cũ (.xls)
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // Cấu hình License cho EPPlus (Export)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // ================= IMPORT (ĐỌC EXCEL) =================
        public List<NhanSu> ImportNhanSu(string filePath)
        {
            var list = new List<NhanSu>();

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Dòng 1 là tiêu đề -> Data bắt đầu từ dòng 2
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);

                        if (dataSet.Tables.Count == 0) throw new Exception("File Excel không có dữ liệu!");

                        var table = dataSet.Tables[0];

                        // Kiểm tra số lượng cột tối thiểu (Cần ít nhất 3 cột: STT, Mã NV, Họ Tên)
                        if (table.Columns.Count < 3) throw new Exception("File Excel thiếu cột dữ liệu!");

                        foreach (DataRow row in table.Rows)
                        {
                            try
                            {
                                // --- ĐỌC DỮ LIỆU AN TOÀN ---
                                // Sử dụng hàm GetSafeString để không bị lỗi nếu thiếu cột

                                string maNv = GetSafeString(row, 1); // Cột B
                                string hoTen = GetSafeString(row, 2); // Cột C

                                // Bỏ qua dòng nếu không có Mã NV hoặc Tên
                                if (string.IsNullOrWhiteSpace(maNv) || string.IsNullOrWhiteSpace(hoTen)) continue;

                                var ns = new NhanSu();
                                ns.MaNhanVien = maNv;
                                ns.HoTen = hoTen;

                                // Ngày sinh (Cột D - index 3)
                                ns.NgaySinh = GetSafeDate(row, 3);

                                // Giới tính (Cột E - index 4)
                                ns.GioiTinh = GetSafeString(row, 4) ?? "Nam";

                                // SĐT (Cột F - index 5)
                                ns.SoDienThoai = GetSafeString(row, 5);

                                // Email (Cột G - index 6)
                                ns.Email = GetSafeString(row, 6);

                                // Địa chỉ (Cột H - index 7)
                                ns.DiaChi = GetSafeString(row, 7);

                                // Ngày vào làm (Cột K - index 10)
                                ns.NgayVaoLam = GetSafeDate(row, 10);

                                // Mức lương (Cột N - index 13)
                                string luongStr = GetSafeString(row, 13);
                                if (!string.IsNullOrEmpty(luongStr))
                                {
                                    luongStr = Regex.Replace(luongStr, "[^0-9]", ""); // Xóa dấu chấm/phẩy
                                    if (decimal.TryParse(luongStr, out decimal luong)) ns.MucLuong = luong;
                                }

                                // --- XỬ LÝ ID MẶC ĐỊNH ---
                                ns.ChucVuId = 3;       // Nhân viên
                                ns.PhongBanId = 1;     // Phòng mặc định
                                ns.LoaiHopDongId = 1;
                                ns.TrinhDoHocVanId = 2; // Đại học
                                ns.TrangThaiId = 1;    // Đang làm việc

                                list.Add(ns);
                            }
                            catch (Exception ex)
                            {
                                // Nếu có dòng lỗi, ghi ra Debug nhưng KHÔNG dừng chương trình
                                System.Diagnostics.Debug.WriteLine("Lỗi dòng: " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ném lỗi ra ngoài để hiển thị MessageBox
                throw new Exception("Lỗi đọc file: " + ex.Message);
            }
            return list;
        }

        // --- HÀM HỖ TRỢ ĐỌC AN TOÀN ---

        // Lấy chuỗi an toàn (tránh lỗi IndexOutOfRange)
        private string GetSafeString(DataRow row, int index)
        {
            if (index >= row.Table.Columns.Count) return null;
            return row[index]?.ToString();
        }

        // Lấy ngày tháng an toàn
        private DateTime? GetSafeDate(DataRow row, int index)
        {
            if (index >= row.Table.Columns.Count) return null;

            object value = row[index];
            if (value == null || value == DBNull.Value) return null;

            if (value is DateTime dt) return dt;

            if (DateTime.TryParse(value.ToString(), out DateTime result))
                return result;

            return null;
        }

        // ================= EXPORT (GHI EXCEL) =================
        public void ExportNhanSu(List<NhanSu> listData, string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách nhân sự");

                // Header
                string[] headers = { "Mã NV", "Họ tên", "Giới tính", "Ngày sinh", "SĐT", "Email", "Chức vụ", "Phòng ban", "Trạng thái" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // Data
                int row = 2;
                foreach (var item in listData)
                {
                    worksheet.Cells[row, 1].Value = item.MaNhanVien;
                    worksheet.Cells[row, 2].Value = item.HoTen;
                    worksheet.Cells[row, 3].Value = item.GioiTinh;
                    worksheet.Cells[row, 4].Value = item.NgaySinh?.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 5].Value = item.SoDienThoai;
                    worksheet.Cells[row, 6].Value = item.Email;
                    worksheet.Cells[row, 7].Value = item.TenChucVu;
                    worksheet.Cells[row, 8].Value = item.TenPhongBan;
                    worksheet.Cells[row, 9].Value = item.TenTrangThai;
                    row++;
                }
                worksheet.Cells.AutoFitColumns();
                package.Save();
            }
        }

        public void ExportDashboard(string filePath, int totalNV, int totalPB, int newNV)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var ws = package.Workbook.Worksheets.Add("Báo cáo");
                ws.Cells["A1"].Value = "BÁO CÁO TỔNG QUAN";
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A3"].Value = "Tổng nhân sự: " + totalNV;
                ws.Cells["A4"].Value = "Tổng phòng ban: " + totalPB;
                ws.Cells["A5"].Value = "Nhân sự mới: " + newNV;
                package.Save();
            }
        }
    }
}