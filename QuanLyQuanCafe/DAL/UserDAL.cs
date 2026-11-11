using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu người dùng / nhân viên (UserDAL)
    /// Cung cấp đầy đủ các thao tác CRUD: GetAll, GetById, Insert, Update, Delete
    /// </summary>
    public class UserDAL
    {
        public List<User> GetAll()
        {
            List<User> list = new List<User>();
            string sql = "SELECT id, email, password, full_name, phone, role, active FROM users ORDER BY id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    Email = row["email"].ToString() ?? string.Empty,
                    Password = row["password"].ToString() ?? string.Empty,
                    FullName = row["full_name"].ToString() ?? string.Empty,
                    Phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : null,
                    Role = row["role"] != DBNull.Value && Convert.ToBoolean(row["role"]),
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        public User? GetById(int id)
        {
            string sql = "SELECT id, email, password, full_name, phone, role, active FROM users WHERE id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    Email = row["email"].ToString() ?? string.Empty,
                    Password = row["password"].ToString() ?? string.Empty,
                    FullName = row["full_name"].ToString() ?? string.Empty,
                    Phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : null,
                    Role = row["role"] != DBNull.Value && Convert.ToBoolean(row["role"]),
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"])
                };
            }
            return null;
        }

        public int Insert(User user)
        {
            string sql = @"INSERT INTO users (email, password, full_name, phone, role, active)
                           VALUES (@1, @2, @3, @4, @5, @6);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                user.Email,
                user.Password,
                user.FullName,
                (object?)user.Phone ?? DBNull.Value,
                user.Role,
                user.Active
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(User user)
        {
            string sql = @"UPDATE users 
                           SET email = @1, password = @2, full_name = @3, phone = @4, role = @5, active = @6
                           WHERE id = @7";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                user.Email,
                user.Password,
                user.FullName,
                (object?)user.Phone ?? DBNull.Value,
                user.Role,
                user.Active,
                user.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM users WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
