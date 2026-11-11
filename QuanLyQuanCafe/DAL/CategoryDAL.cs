using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu danh mục (CategoryDAL)
    /// Cung cấp đầy đủ các thao tác CRUD: GetAll, GetById, Insert, Update, Delete
    /// </summary>
    public class CategoryDAL
    {
        public List<Category> GetAll()
        {
            List<Category> list = new List<Category>();
            string sql = "SELECT id, name, active FROM categories ORDER BY id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Category
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString() ?? string.Empty,
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        public Category? GetById(int id)
        {
            string sql = "SELECT id, name, active FROM categories WHERE id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Category
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString() ?? string.Empty,
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"])
                };
            }
            return null;
        }

        public int Insert(Category category)
        {
            string sql = "INSERT INTO categories (name, active) VALUES (@1, @2); SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object> { category.Name, category.Active });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Category category)
        {
            string sql = "UPDATE categories SET name = @1, active = @2 WHERE id = @3";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { category.Name, category.Active, category.Id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM categories WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
