using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class ReaderDAL
    {
        private readonly LibraryDbContext _context;

        public ReaderDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public List<Reader> GetAll()
        {
            return _context.Readers
                .OrderBy(r => r.Id)
                .AsNoTracking()
                .ToList();
        }

        public Reader? GetById(int id)
        {
            return _context.Readers.Find(id);
        }

        public int Insert(Reader reader)
        {
            _context.Readers.Add(reader);
            _context.SaveChanges();
            return reader.Id;
        }

        public bool Update(Reader reader)
        {
            var existing = _context.Readers.Find(reader.Id);
            if (existing == null) return false;

            existing.ReaderCode = reader.ReaderCode;
            existing.FullName = reader.FullName;
            existing.Email = reader.Email;
            existing.Password = reader.Password;
            existing.Phone = reader.Phone;
            existing.Address = reader.Address;
            existing.IdentityNumber = reader.IdentityNumber;
            existing.ExpiryDate = reader.ExpiryDate;
            existing.MaxBorrowLimit = reader.MaxBorrowLimit;
            existing.Active = reader.Active;

            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var existing = _context.Readers.Find(id);
            if (existing == null) return false;

            _context.Readers.Remove(existing);
            return _context.SaveChanges() > 0;
        }
    }
}
