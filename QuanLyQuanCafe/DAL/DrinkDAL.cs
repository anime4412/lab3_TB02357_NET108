using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu đồ uống (DrinkDAL)
    /// Cung cấp đầy đủ các thao tác CRUD: GetAll, GetById, Insert, Update, Delete
    /// </summary>
    public class DrinkDAL
    {
        public List<Drink> GetAll()
        {
            List<Drink> list = new List<Drink>();
            string sql = @"SELECT d.id, d.category_id, d.name, d.price, d.image, d.description, d.active,
                                  c.name AS category_name
                           FROM drinks d
                           LEFT JOIN categories c ON d.category_id = c.id
                           ORDER BY d.id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Drink
                {
                    Id = Convert.ToInt32(row["id"]),
                    CategoryId = row["category_id"] != DBNull.Value ? Convert.ToInt32(row["category_id"]) : null,
                    Name = row["name"].ToString() ?? string.Empty,
                    Price = Convert.ToInt32(row["price"]),
                    Image = row["image"] != DBNull.Value ? row["image"].ToString() : null,
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"]),
                    CategoryName = row["category_name"] != DBNull.Value ? row["category_name"].ToString() : null
                });
            }
            return list;
        }

        public Drink? GetById(int id)
        {
            string sql = @"SELECT d.id, d.category_id, d.name, d.price, d.image, d.description, d.active,
                                  c.name AS category_name
                           FROM drinks d
                           LEFT JOIN categories c ON d.category_id = c.id
                           WHERE d.id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Drink
                {
                    Id = Convert.ToInt32(row["id"]),
                    CategoryId = row["category_id"] != DBNull.Value ? Convert.ToInt32(row["category_id"]) : null,
                    Name = row["name"].ToString() ?? string.Empty,
                    Price = Convert.ToInt32(row["price"]),
                    Image = row["image"] != DBNull.Value ? row["image"].ToString() : null,
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"]),
                    CategoryName = row["category_name"] != DBNull.Value ? row["category_name"].ToString() : null
                };
            }
            return null;
        }

        public int Insert(Drink drink)
        {
            string sql = @"INSERT INTO drinks (category_id, name, price, image, description, active)
                           VALUES (@1, @2, @3, @4, @5, @6);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                (object?)drink.CategoryId ?? DBNull.Value,
                drink.Name,
                drink.Price,
                (object?)drink.Image ?? DBNull.Value,
                (object?)drink.Description ?? DBNull.Value,
                drink.Active
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Drink drink)
        {
            string sql = @"UPDATE drinks 
                           SET category_id = @1, name = @2, price = @3, image = @4, description = @5, active = @6
                           WHERE id = @7";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                (object?)drink.CategoryId ?? DBNull.Value,
                drink.Name,
                drink.Price,
                (object?)drink.Image ?? DBNull.Value,
                (object?)drink.Description ?? DBNull.Value,
                drink.Active,
                drink.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM drinks WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
