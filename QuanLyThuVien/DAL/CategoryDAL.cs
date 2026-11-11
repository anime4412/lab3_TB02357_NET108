using System;
using System.Collections.Generic;
using System.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class CategoryDAL
    {
        public List<Category> GetAll()
        {
            List<Category> list = new List<Category>();
            string sql = "SELECT Id, Name, Description, Active FROM Categories ORDER BY Id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Category
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString() ?? string.Empty,
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Active = row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"])
                });
            }
            return list;
        }

        public Category? GetById(int id)
        {
            string sql = "SELECT Id, Name, Description, Active FROM Categories WHERE Id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Category
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString() ?? string.Empty,
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Active = row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"])
                };
            }
            return null;
        }

        public int Insert(Category cat)
        {
            string sql = "INSERT INTO Categories (Name, Description, Active) VALUES (@1, @2, @3); SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object> { cat.Name, (object?)cat.Description ?? DBNull.Value, cat.Active });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Category cat)
        {
            string sql = "UPDATE Categories SET Name = @1, Description = @2, Active = @3 WHERE Id = @4";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { cat.Name, (object?)cat.Description ?? DBNull.Value, cat.Active, cat.Id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM Categories WHERE Id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
