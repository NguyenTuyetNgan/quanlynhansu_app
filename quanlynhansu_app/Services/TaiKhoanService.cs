using MySql.Data.MySqlClient;
using quanlynhansu_app.Data;
using quanlynhansu_app.Models;
using System.Collections.Generic;
using System.Data;
using System;

namespace quanlynhansu_app.Services
{
    public class TaiKhoanService
    {
        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();
            string query = @"SELECT * FROM users ORDER BY created_at DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    Username = row["username"].ToString(),
                    Email = row["email"].ToString(),
                    Role = row["role"].ToString(),
                    // Không lấy password ra UI để bảo mật
                });
            }
            return list;
        }

        public bool CreateUser(string username, string password, string email, string role)
        {
            // Trong thực tế nên mã hóa password (MD5/BCrypt)
            string query = "INSERT INTO users (username, password, email, role) VALUES (@User, @Pass, @Email, @Role)";
            var param = new MySqlParameter[] {
                new MySqlParameter("@User", username),
                new MySqlParameter("@Pass", password), // Hash password ở đây nếu cần
                new MySqlParameter("@Email", email),
                new MySqlParameter("@Role", role)
            };
            return DatabaseHelper.ExecuteNonQuery(query, param) > 0;
        }

        public bool DeleteUser(int id)
        {
            string query = "DELETE FROM users WHERE id = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@Id", id)) > 0;
        }

        public bool ResetPassword(int id, string newPass)
        {
            string query = "UPDATE users SET password = @Pass WHERE id = @Id";
            var param = new MySqlParameter[] {
                new MySqlParameter("@Id", id),
                new MySqlParameter("@Pass", newPass)
            };
            return DatabaseHelper.ExecuteNonQuery(query, param) > 0;
        }
    }
}