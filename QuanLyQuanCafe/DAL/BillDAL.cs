using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu hóa đơn (BillDAL)
    /// </summary>
    public class BillDAL
    {
        public List<Bill> GetAll()
        {
            List<Bill> list = new List<Bill>();
            string sql = @"SELECT b.id, b.user_id, b.card_id, b.code, b.created_at, b.total, b.status,
                                  u.full_name AS user_name, c.code AS card_code
                           FROM bills b
                           LEFT JOIN users u ON b.user_id = u.id
                           LEFT JOIN cards c ON b.card_id = c.id
                           ORDER BY b.id DESC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Bill
                {
                    Id = Convert.ToInt32(row["id"]),
                    UserId = row["user_id"] != DBNull.Value ? Convert.ToInt32(row["user_id"]) : null,
                    CardId = row["card_id"] != DBNull.Value ? Convert.ToInt32(row["card_id"]) : null,
                    Code = row["code"].ToString() ?? string.Empty,
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    Total = Convert.ToInt32(row["total"]),
                    Status = row["status"].ToString() ?? string.Empty,
                    UserName = row["user_name"] != DBNull.Value ? row["user_name"].ToString() : null,
                    CardCode = row["card_code"] != DBNull.Value ? Convert.ToInt32(row["card_code"]) : null
                });
            }
            return list;
        }

        public Bill? GetById(int id)
        {
            string sql = @"SELECT b.id, b.user_id, b.card_id, b.code, b.created_at, b.total, b.status,
                                  u.full_name AS user_name, c.code AS card_code
                           FROM bills b
                           LEFT JOIN users u ON b.user_id = u.id
                           LEFT JOIN cards c ON b.card_id = c.id
                           WHERE b.id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Bill
                {
                    Id = Convert.ToInt32(row["id"]),
                    UserId = row["user_id"] != DBNull.Value ? Convert.ToInt32(row["user_id"]) : null,
                    CardId = row["card_id"] != DBNull.Value ? Convert.ToInt32(row["card_id"]) : null,
                    Code = row["code"].ToString() ?? string.Empty,
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    Total = Convert.ToInt32(row["total"]),
                    Status = row["status"].ToString() ?? string.Empty,
                    UserName = row["user_name"] != DBNull.Value ? row["user_name"].ToString() : null,
                    CardCode = row["card_code"] != DBNull.Value ? Convert.ToInt32(row["card_code"]) : null
                };
            }
            return null;
        }

        public int Insert(Bill bill)
        {
            string sql = @"INSERT INTO bills (user_id, card_id, code, created_at, total, status)
                           VALUES (@1, @2, @3, @4, @5, @6);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                (object?)bill.UserId ?? DBNull.Value,
                (object?)bill.CardId ?? DBNull.Value,
                bill.Code,
                bill.CreatedAt,
                bill.Total,
                bill.Status
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Bill bill)
        {
            string sql = @"UPDATE bills 
                           SET user_id = @1, card_id = @2, code = @3, created_at = @4, total = @5, status = @6
                           WHERE id = @7";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                (object?)bill.UserId ?? DBNull.Value,
                (object?)bill.CardId ?? DBNull.Value,
                bill.Code,
                bill.CreatedAt,
                bill.Total,
                bill.Status,
                bill.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM bills WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
