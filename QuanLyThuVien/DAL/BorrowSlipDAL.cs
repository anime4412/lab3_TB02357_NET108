using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class BorrowSlipDAL
    {
        private readonly LibraryDbContext _context;

        public BorrowSlipDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public List<BorrowSlip> GetAll()
        {
            return _context.BorrowSlips
                .Include(bs => bs.Reader)
                .Include(bs => bs.Employee)
                .OrderByDescending(bs => bs.Id)
                .AsNoTracking()
                .ToList();
        }

        public BorrowSlip? GetById(int id)
        {
            return _context.BorrowSlips
                .Include(bs => bs.Reader)
                .Include(bs => bs.Employee)
                .Include(bs => bs.BorrowDetails)
                .FirstOrDefault(bs => bs.Id == id);
        }

        public int Insert(BorrowSlip slip)
        {
            _context.BorrowSlips.Add(slip);
            _context.SaveChanges();
            return slip.Id;
        }

        public bool Update(BorrowSlip slip)
        {
            var existing = _context.BorrowSlips.Find(slip.Id);
            if (existing == null) return false;

            existing.SlipCode = slip.SlipCode;
            existing.ReaderId = slip.ReaderId;
            existing.EmployeeId = slip.EmployeeId;
            existing.DueDate = slip.DueDate;
            existing.ActualReturnDate = slip.ActualReturnDate;
            existing.Status = slip.Status;
            existing.DepositAmount = slip.DepositAmount;
            existing.Notes = slip.Notes;

            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var existing = _context.BorrowSlips.Find(id);
            if (existing == null) return false;

            _context.BorrowSlips.Remove(existing);
            return _context.SaveChanges() > 0;
        }
    }
}
