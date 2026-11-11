using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyThuVien.DAL
{
    /// <summary>
    /// BÀI 3: Lớp tiện ích DBUtil hỗ trợ truy vấn và tương tác CSDL bằng ADO.NET
    /// Đề tài: Hệ thống Quản lý Thư viện Điện tử (Library Management System)
    /// Tự động gán tham số @1, @2,... chống tấn công SQL Injection và bảo toàn Unicode
    /// </summary>
    public static class DBUtil
    {
        // Chuỗi kết nối CSDL SQL Server mặc định
        public static string connString = "Server=localhost;Database=QuanLyThuVien;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

        /// <summary>
        /// Thực thi câu lệnh SQL SELECT trả về bảng dữ liệu DataTable
        /// </summary>
        public static DataTable queryDataTable(string sql, List<object>? parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Thêm tham số vào SQL nếu có (sử dụng NVARCHAR cho string để bảo toàn Unicode)
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                var paramName = $"@{i + 1}";
                                var value = parameters[i] ?? DBNull.Value;
                                if (value is string)
                                {
                                    var p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                    p.Value = value;
                                    cmd.Parameters.Add(p);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(paramName, value);
                                }
                            }
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi QueryDataTable: {ex.Message}");
                    throw;
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Thực thi câu lệnh SQL và lấy về giá trị đơn lẻ (cột 1 dòng 1)
        /// Thường dùng cho SELECT COUNT(*), MAX(), SUM() hoặc lấy SCOPE_IDENTITY()
        /// </summary>
        public static object? executeScalar(string sql, List<object>? parameters = null)
        {
            object? result = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                var paramName = $"@{i + 1}";
                                var value = parameters[i] ?? DBNull.Value;
                                if (value is string)
                                {
                                    var p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                    p.Value = value;
                                    cmd.Parameters.Add(p);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(paramName, value);
                                }
                            }
                        }

                        result = cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi executeScalar: {ex.Message}");
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Thực thi các câu lệnh thay đổi dữ liệu: INSERT, UPDATE, DELETE
        /// Trả về số dòng bị tác động (rows affected)
        /// </summary>
        public static int executeNonQuery(string sql, List<object>? parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                var paramName = $"@{i + 1}";
                                var value = parameters[i] ?? DBNull.Value;
                                if (value is string)
                                {
                                    var p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                    p.Value = value;
                                    cmd.Parameters.Add(p);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(paramName, value);
                                }
                            }
                        }

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi executeNonQuery: {ex.Message}");
                    throw;
                }
            }
            return rowsAffected;
        }
    }
}
