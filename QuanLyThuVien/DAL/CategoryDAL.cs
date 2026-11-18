using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class CategoryDAL
    {
        private readonly LibraryDbContext _context;

        public CategoryDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll()
        {
            return _context.Categories
                .OrderBy(c => c.Id)
                .AsNoTracking()
                .ToList();
        }

        public Category? GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public int Insert(Category cat)
        {
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return cat.Id;
        }

        public bool Update(Category cat)
        {
            var existing = _context.Categories.Find(cat.Id);
            if (existing == null) return false;

            existing.Name = cat.Name;
            existing.Description = cat.Description;
            existing.Active = cat.Active;

            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var existing = _context.Categories.Find(id);
            if (existing == null) return false;

            _context.Categories.Remove(existing);
            return _context.SaveChanges() > 0;
        }
    }
}
