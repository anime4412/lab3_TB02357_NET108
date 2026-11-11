using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu chi tiết hóa đơn (BillDetailDAL)
    /// </summary>
    public class BillDetailDAL
    {
        public List<BillDetail> GetAll()
        {
            List<BillDetail> list = new List<BillDetail>();
            string sql = @"SELECT bd.id, bd.bill_id, bd.drink_id, bd.quantity, bd.price,
                                  d.name AS drink_name
                           FROM bill_details bd
                           LEFT JOIN drinks d ON bd.drink_id = d.id
                           ORDER BY bd.id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BillDetail
                {
                    Id = Convert.ToInt32(row["id"]),
                    BillId = Convert.ToInt32(row["bill_id"]),
                    DrinkId = Convert.ToInt32(row["drink_id"]),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    Price = Convert.ToInt32(row["price"]),
                    DrinkName = row["drink_name"] != DBNull.Value ? row["drink_name"].ToString() : null
                });
            }
            return list;
        }

        public List<BillDetail> GetByBillId(int billId)
        {
            List<BillDetail> list = new List<BillDetail>();
            string sql = @"SELECT bd.id, bd.bill_id, bd.drink_id, bd.quantity, bd.price,
                                  d.name AS drink_name
                           FROM bill_details bd
                           LEFT JOIN drinks d ON bd.drink_id = d.id
                           WHERE bd.bill_id = @1
                           ORDER BY bd.id ASC";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { billId });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BillDetail
                {
                    Id = Convert.ToInt32(row["id"]),
                    BillId = Convert.ToInt32(row["bill_id"]),
                    DrinkId = Convert.ToInt32(row["drink_id"]),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    Price = Convert.ToInt32(row["price"]),
                    DrinkName = row["drink_name"] != DBNull.Value ? row["drink_name"].ToString() : null
                });
            }
            return list;
        }

        public BillDetail? GetById(int id)
        {
            string sql = @"SELECT bd.id, bd.bill_id, bd.drink_id, bd.quantity, bd.price,
                                  d.name AS drink_name
                           FROM bill_details bd
                           LEFT JOIN drinks d ON bd.drink_id = d.id
                           WHERE bd.id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new BillDetail
                {
                    Id = Convert.ToInt32(row["id"]),
                    BillId = Convert.ToInt32(row["bill_id"]),
                    DrinkId = Convert.ToInt32(row["drink_id"]),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    Price = Convert.ToInt32(row["price"]),
                    DrinkName = row["drink_name"] != DBNull.Value ? row["drink_name"].ToString() : null
                };
            }
            return null;
        }

        public int Insert(BillDetail detail)
        {
            string sql = @"INSERT INTO bill_details (bill_id, drink_id, quantity, price)
                           VALUES (@1, @2, @3, @4);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                detail.BillId,
                detail.DrinkId,
                detail.Quantity,
                detail.Price
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(BillDetail detail)
        {
            string sql = @"UPDATE bill_details 
                           SET bill_id = @1, drink_id = @2, quantity = @3, price = @4
                           WHERE id = @5";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                detail.BillId,
                detail.DrinkId,
                detail.Quantity,
                detail.Price,
                detail.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM bill_details WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
