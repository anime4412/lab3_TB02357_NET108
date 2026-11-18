using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu Sách (BookDAL) sử dụng EF Core Code First & LINQ
    /// Đầy đủ 5 thao tác CRUD: GetAll, GetById, Insert, Update, Delete
    /// </summary>
    public class BookDAL
    {
        private readonly LibraryDbContext _context;

        public BookDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Shelf)
                .OrderByDescending(b => b.Id)
                .AsNoTracking()
                .ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Shelf)
                .FirstOrDefault(b => b.Id == id);
        }

        public int Insert(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book.Id;
        }

        public bool Update(Book book)
        {
            var existing = _context.Books.Find(book.Id);
            if (existing == null) return false;

            existing.ISBN = book.ISBN;
            existing.Title = book.Title;
            existing.CategoryId = book.CategoryId;
            existing.AuthorId = book.AuthorId;
            existing.PublisherId = book.PublisherId;
            existing.ShelfId = book.ShelfId;
            existing.PublishYear = book.PublishYear;
            existing.PageCount = book.PageCount;
            existing.Price = book.Price;
            existing.Quantity = book.Quantity;
            existing.AvailableQuantity = book.AvailableQuantity;
            existing.CoverImage = book.CoverImage;
            existing.Description = book.Description;
            existing.Active = book.Active;

            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var existing = _context.Books.Find(id);
            if (existing == null) return false;

            _context.Books.Remove(existing);
            return _context.SaveChanges() > 0;
        }
    }
}
