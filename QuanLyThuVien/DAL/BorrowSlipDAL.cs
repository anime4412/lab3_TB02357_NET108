using System;
using System.Collections.Generic;
using System.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class BorrowSlipDAL
    {
        public List<BorrowSlip> GetAll()
        {
            List<BorrowSlip> list = new List<BorrowSlip>();
            string sql = @"SELECT b.Id, b.SlipCode, b.ReaderId, b.EmployeeId, b.BorrowDate, b.DueDate, 
                                  b.ActualReturnDate, b.Status, b.DepositAmount, b.Notes,
                                  r.FullName AS ReaderName, e.FullName AS EmployeeName
                           FROM BorrowSlips b
                           LEFT JOIN Readers r ON b.ReaderId = r.Id
                           LEFT JOIN Employees e ON b.EmployeeId = e.Id
                           ORDER BY b.Id DESC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BorrowSlip
                {
                    Id = Convert.ToInt32(row["Id"]),
                    SlipCode = row["SlipCode"].ToString() ?? string.Empty,
                    ReaderId = Convert.ToInt32(row["ReaderId"]),
                    EmployeeId = row["EmployeeId"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : null,
                    BorrowDate = Convert.ToDateTime(row["BorrowDate"]),
                    DueDate = Convert.ToDateTime(row["DueDate"]),
                    ActualReturnDate = row["ActualReturnDate"] != DBNull.Value ? Convert.ToDateTime(row["ActualReturnDate"]) : null,
                    Status = row["Status"].ToString() ?? string.Empty,
                    DepositAmount = Convert.ToDecimal(row["DepositAmount"]),
                    Notes = row["Notes"] != DBNull.Value ? row["Notes"].ToString() : null,
                    ReaderName = row["ReaderName"] != DBNull.Value ? row["ReaderName"].ToString() : null,
                    EmployeeName = row["EmployeeName"] != DBNull.Value ? row["EmployeeName"].ToString() : null
                });
            }
            return list;
        }

        public BorrowSlip? GetById(int id)
        {
            string sql = @"SELECT b.Id, b.SlipCode, b.ReaderId, b.EmployeeId, b.BorrowDate, b.DueDate, 
                                  b.ActualReturnDate, b.Status, b.DepositAmount, b.Notes,
                                  r.FullName AS ReaderName, e.FullName AS EmployeeName
                           FROM BorrowSlips b
                           LEFT JOIN Readers r ON b.ReaderId = r.Id
                           LEFT JOIN Employees e ON b.EmployeeId = e.Id
                           WHERE b.Id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new BorrowSlip
                {
                    Id = Convert.ToInt32(row["Id"]),
                    SlipCode = row["SlipCode"].ToString() ?? string.Empty,
                    ReaderId = Convert.ToInt32(row["ReaderId"]),
                    EmployeeId = row["EmployeeId"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : null,
                    BorrowDate = Convert.ToDateTime(row["BorrowDate"]),
                    DueDate = Convert.ToDateTime(row["DueDate"]),
                    ActualReturnDate = row["ActualReturnDate"] != DBNull.Value ? Convert.ToDateTime(row["ActualReturnDate"]) : null,
                    Status = row["Status"].ToString() ?? string.Empty,
                    DepositAmount = Convert.ToDecimal(row["DepositAmount"]),
                    Notes = row["Notes"] != DBNull.Value ? row["Notes"].ToString() : null,
                    ReaderName = row["ReaderName"] != DBNull.Value ? row["ReaderName"].ToString() : null,
                    EmployeeName = row["EmployeeName"] != DBNull.Value ? row["EmployeeName"].ToString() : null
                };
            }
            return null;
        }

        public int Insert(BorrowSlip slip)
        {
            string sql = @"INSERT INTO BorrowSlips (SlipCode, ReaderId, EmployeeId, BorrowDate, DueDate, Status, DepositAmount, Notes)
                           VALUES (@1, @2, @3, @4, @5, @6, @7, @8);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                slip.SlipCode,
                slip.ReaderId,
                (object?)slip.EmployeeId ?? DBNull.Value,
                slip.BorrowDate,
                slip.DueDate,
                slip.Status,
                slip.DepositAmount,
                (object?)slip.Notes ?? DBNull.Value
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(BorrowSlip slip)
        {
            string sql = @"UPDATE BorrowSlips 
                           SET SlipCode = @1, ReaderId = @2, EmployeeId = @3, DueDate = @4, 
                               ActualReturnDate = @5, Status = @6, DepositAmount = @7, Notes = @8
                           WHERE Id = @9";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                slip.SlipCode,
                slip.ReaderId,
                (object?)slip.EmployeeId ?? DBNull.Value,
                slip.DueDate,
                (object?)slip.ActualReturnDate ?? DBNull.Value,
                slip.Status,
                slip.DepositAmount,
                (object?)slip.Notes ?? DBNull.Value,
                slip.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM BorrowSlips WHERE Id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
