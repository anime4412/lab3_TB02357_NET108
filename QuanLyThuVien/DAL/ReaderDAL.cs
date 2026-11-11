using System;
using System.Collections.Generic;
using System.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class ReaderDAL
    {
        public List<Reader> GetAll()
        {
            List<Reader> list = new List<Reader>();
            string sql = "SELECT Id, ReaderCode, FullName, Email, Password, Phone, Address, IdentityNumber, RegisterDate, ExpiryDate, MaxBorrowLimit, Active FROM Readers ORDER BY Id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Reader
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ReaderCode = row["ReaderCode"].ToString() ?? string.Empty,
                    FullName = row["FullName"].ToString() ?? string.Empty,
                    Email = row["Email"].ToString() ?? string.Empty,
                    Password = row["Password"].ToString() ?? string.Empty,
                    Phone = row["Phone"] != DBNull.Value ? row["Phone"].ToString() : null,
                    Address = row["Address"] != DBNull.Value ? row["Address"].ToString() : null,
                    IdentityNumber = row["IdentityNumber"] != DBNull.Value ? row["IdentityNumber"].ToString() : null,
                    RegisterDate = Convert.ToDateTime(row["RegisterDate"]),
                    ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]),
                    MaxBorrowLimit = Convert.ToInt32(row["MaxBorrowLimit"]),
                    Active = row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"])
                });
            }
            return list;
        }

        public Reader? GetById(int id)
        {
            string sql = "SELECT Id, ReaderCode, FullName, Email, Password, Phone, Address, IdentityNumber, RegisterDate, ExpiryDate, MaxBorrowLimit, Active FROM Readers WHERE Id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Reader
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ReaderCode = row["ReaderCode"].ToString() ?? string.Empty,
                    FullName = row["FullName"].ToString() ?? string.Empty,
                    Email = row["Email"].ToString() ?? string.Empty,
                    Password = row["Password"].ToString() ?? string.Empty,
                    Phone = row["Phone"] != DBNull.Value ? row["Phone"].ToString() : null,
                    Address = row["Address"] != DBNull.Value ? row["Address"].ToString() : null,
                    IdentityNumber = row["IdentityNumber"] != DBNull.Value ? row["IdentityNumber"].ToString() : null,
                    RegisterDate = Convert.ToDateTime(row["RegisterDate"]),
                    ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]),
                    MaxBorrowLimit = Convert.ToInt32(row["MaxBorrowLimit"]),
                    Active = row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"])
                };
            }
            return null;
        }

        public int Insert(Reader reader)
        {
            string sql = @"INSERT INTO Readers (ReaderCode, FullName, Email, Password, Phone, Address, IdentityNumber, RegisterDate, ExpiryDate, MaxBorrowLimit, Active)
                           VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                reader.ReaderCode,
                reader.FullName,
                reader.Email,
                reader.Password,
                (object?)reader.Phone ?? DBNull.Value,
                (object?)reader.Address ?? DBNull.Value,
                (object?)reader.IdentityNumber ?? DBNull.Value,
                reader.RegisterDate,
                reader.ExpiryDate,
                reader.MaxBorrowLimit,
                reader.Active
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Reader reader)
        {
            string sql = @"UPDATE Readers 
                           SET ReaderCode = @1, FullName = @2, Email = @3, Password = @4, Phone = @5, 
                               Address = @6, IdentityNumber = @7, ExpiryDate = @8, MaxBorrowLimit = @9, Active = @10
                           WHERE Id = @11";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                reader.ReaderCode,
                reader.FullName,
                reader.Email,
                reader.Password,
                (object?)reader.Phone ?? DBNull.Value,
                (object?)reader.Address ?? DBNull.Value,
                (object?)reader.IdentityNumber ?? DBNull.Value,
                reader.ExpiryDate,
                reader.MaxBorrowLimit,
                reader.Active,
                reader.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM Readers WHERE Id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
