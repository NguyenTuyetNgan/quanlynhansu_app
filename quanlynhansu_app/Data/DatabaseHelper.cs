using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace quanlynhansu_app.Data
{
    /// <summary>
    /// Lớp quản lý kết nối và thao tác với MySQL Database
    /// </summary>
    public class DatabaseHelper
    {
        // Connection string - thay đổi theo cấu hình của bạn
        private static string connectionString = "Server=localhost;Database=quanlynhansu_db;Uid=root;Pwd=;CharSet=utf8mb4;";

        /// <summary>
        /// Lấy connection mới đến database
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi kết nối: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT và trả về DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        // Thêm parameters nếu có
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi ExecuteQuery: {ex.Message}");
            }

            return dataTable;
        }

        /// <summary>
        /// Thực thi câu lệnh INSERT, UPDATE, DELETE
        /// Trả về số dòng bị ảnh hưởng
        /// </summary>
        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            int rowsAffected = 0;

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        // Thêm parameters nếu có
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi ExecuteNonQuery: {ex.Message}");
            }

            return rowsAffected;
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT trả về giá trị đơn (COUNT, SUM, etc.)
        /// </summary>
        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            object result = null;

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        // Thêm parameters nếu có
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi ExecuteScalar: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Lấy ID vừa insert (LastInsertId)
        /// </summary>
        public static long GetLastInsertId(MySqlConnection conn)
        {
            using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
            {
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }
    }
}