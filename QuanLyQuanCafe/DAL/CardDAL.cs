using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu thẻ bàn / thẻ rung (CardDAL)
    /// </summary>
    public class CardDAL
    {
        public List<Card> GetAll()
        {
            List<Card> list = new List<Card>();
            string sql = "SELECT id, code, status FROM cards ORDER BY id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Card
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = Convert.ToInt32(row["code"]),
                    Status = row["status"] != DBNull.Value && Convert.ToBoolean(row["status"])
                });
            }
            return list;
        }

        public Card? GetById(int id)
        {
            string sql = "SELECT id, code, status FROM cards WHERE id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Card
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = Convert.ToInt32(row["code"]),
                    Status = row["status"] != DBNull.Value && Convert.ToBoolean(row["status"])
                };
            }
            return null;
        }

        public int Insert(Card card)
        {
            string sql = "INSERT INTO cards (code, status) VALUES (@1, @2); SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object> { card.Code, card.Status });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Card card)
        {
            string sql = "UPDATE cards SET code = @1, status = @2 WHERE id = @3";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { card.Code, card.Status, card.Id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM cards WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
