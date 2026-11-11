using System;
using System.Collections.Generic;
using System.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    /// <summary>
    /// BÀI 4: Lớp truy xuất dữ liệu Sách (BookDAL)
    /// Đề tài: Hệ thống Quản lý Thư viện Điện tử
    /// Thực hiện đầy đủ 5 thao tác CRUD: GetAll, GetById, Insert, Update, Delete
    /// </summary>
    public class BookDAL
    {
        public List<Book> GetAll()
        {
            List<Book> list = new List<Book>();
            string sql = @"SELECT b.Id, b.ISBN, b.Title, b.CategoryId, b.AuthorId, b.PublisherId, b.ShelfId,
                                  b.PublishYear, b.PageCount, b.Price, b.Quantity, b.AvailableQuantity,
                                  b.CoverImage, b.Description, b.Active,
                                  c.Name AS CategoryName, a.Name AS AuthorName, 
                                  p.Name AS PublisherName, s.Name AS ShelfName
                           FROM Books b
                           LEFT JOIN Categories c ON b.CategoryId = c.Id
                           LEFT JOIN Authors a ON b.AuthorId = a.Id
                           LEFT JOIN Publishers p ON b.PublisherId = p.Id
                           LEFT JOIN Shelves s ON b.ShelfId = s.Id
                           ORDER BY b.Id DESC";
            DataTable dt = DBUtil.queryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Book
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ISBN = row["ISBN"].ToString() ?? string.Empty,
                    Title = row["Title"].ToString() ?? string.Empty,
                    CategoryId = row["CategoryId"] != DBNull.Value ? Convert.ToInt32(row["CategoryId"]) : null,
                    AuthorId = row["AuthorId"] != DBNull.Value ? Convert.ToInt32(row["AuthorId"]) : null,
                    PublisherId = row["PublisherId"] != DBNull.Value ? Convert.ToInt32(row["PublisherId"]) : null,
                    ShelfId = row["ShelfId"] != DBNull.Value ? Convert.ToInt32(row["ShelfId"]) : null,
                    PublishYear = row["PublishYear"] != DBNull.Value ? Convert.ToInt32(row["PublishYear"]) : 2024,
                    PageCount = row["PageCount"] != DBNull.Value ? Convert.ToInt32(row["PageCount"]) : 0,
                    Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0,
                    Quantity = row["Quantity"] != DBNull.Value ? Convert.ToInt32(row["Quantity"]) : 0,
                    AvailableQuantity = row["AvailableQuantity"] != DBNull.Value ? Convert.ToInt32(row["AvailableQuantity"]) : 0,
                    CoverImage = row["CoverImage"] != DBNull.Value ? row["CoverImage"].ToString() : null,
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Active = row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]),
                    CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : null,
                    AuthorName = row["AuthorName"] != DBNull.Value ? row["AuthorName"].ToString() : null,
                    PublisherName = row["PublisherName"] != DBNull.Value ? row["PublisherName"].ToString() : null,
                    ShelfName = row["ShelfName"] != DBNull.Value ? row["ShelfName"].ToString() : null
                });
            }
            return list;
        }

        public Book? GetById(int id)
        {
            string sql = @"SELECT b.Id, b.ISBN, b.Title, b.CategoryId, b.AuthorId, b.PublisherId, b.ShelfId,
                                  b.PublishYear, b.PageCount, b.Price, b.Quantity, b.AvailableQuantity,
                                  b.CoverImage, b.Description, b.Active,
                                  c.Name AS CategoryName, a.Name AS AuthorName, 
                                  p.Name AS PublisherName, s.Name AS ShelfName
                           FROM Books b
                           LEFT JOIN Categories c ON b.CategoryId = c.Id
                           LEFT JOIN Authors a ON b.AuthorId = a.Id
                           LEFT JOIN Publishers p ON b.PublisherId = p.Id
                           LEFT JOIN Shelves s ON b.ShelfId = s.Id
                           WHERE b.Id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Book
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ISBN = row["ISBN"].ToString() ?? string.Empty,
                    Title = row["Title"].ToString() ?? string.Empty,
                    CategoryId = row["CategoryId"] != DBNull.Value ? Convert.ToInt32(row["CategoryId"]) : null,
                    AuthorId = row["AuthorId"] != DBNull.Value ? Convert.ToInt32(row["AuthorId"]) : null,
                    PublisherId = row["PublisherId"] != DBNull.Value ? Convert.ToInt32(row["PublisherId"]) : null,
                    ShelfId = row["ShelfId"] != DBNull.Value ? Convert.ToInt32(row["ShelfId"]) : null,
                    PublishYear = row["PublishYear"] != DBNull.Value ? Convert.ToInt32(row["PublishYear"]) : 2024,
                    PageCount = row["PageCount"] != DBNull.Value ? Convert.ToInt32(row["PageCount"]) : 0,
                    Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0,
                    Quantity = row["Quantity"] != DBNull.Value ? Convert.ToInt32(row["Quantity"]) : 0,
                    AvailableQuantity = row["AvailableQuantity"] != DBNull.Value ? Convert.ToInt32(row["AvailableQuantity"]) : 0,
                    CoverImage = row["CoverImage"] != DBNull.Value ? row["CoverImage"].ToString() : null,
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Active = row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]),
                    CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : null,
                    AuthorName = row["AuthorName"] != DBNull.Value ? row["AuthorName"].ToString() : null,
                    PublisherName = row["PublisherName"] != DBNull.Value ? row["PublisherName"].ToString() : null,
                    ShelfName = row["ShelfName"] != DBNull.Value ? row["ShelfName"].ToString() : null
                };
            }
            return null;
        }

        public int Insert(Book book)
        {
            string sql = @"INSERT INTO Books (ISBN, Title, CategoryId, AuthorId, PublisherId, ShelfId, 
                                              PublishYear, PageCount, Price, Quantity, AvailableQuantity, 
                                              CoverImage, Description, Active)
                           VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                book.ISBN,
                book.Title,
                (object?)book.CategoryId ?? DBNull.Value,
                (object?)book.AuthorId ?? DBNull.Value,
                (object?)book.PublisherId ?? DBNull.Value,
                (object?)book.ShelfId ?? DBNull.Value,
                book.PublishYear,
                book.PageCount,
                book.Price,
                book.Quantity,
                book.AvailableQuantity,
                (object?)book.CoverImage ?? DBNull.Value,
                (object?)book.Description ?? DBNull.Value,
                book.Active
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public bool Update(Book book)
        {
            string sql = @"UPDATE Books 
                           SET ISBN = @1, Title = @2, CategoryId = @3, AuthorId = @4, PublisherId = @5, 
                               ShelfId = @6, PublishYear = @7, PageCount = @8, Price = @9, 
                               Quantity = @10, AvailableQuantity = @11, CoverImage = @12, 
                               Description = @13, Active = @14
                           WHERE Id = @15";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                book.ISBN,
                book.Title,
                (object?)book.CategoryId ?? DBNull.Value,
                (object?)book.AuthorId ?? DBNull.Value,
                (object?)book.PublisherId ?? DBNull.Value,
                (object?)book.ShelfId ?? DBNull.Value,
                book.PublishYear,
                book.PageCount,
                book.Price,
                book.Quantity,
                book.AvailableQuantity,
                (object?)book.CoverImage ?? DBNull.Value,
                (object?)book.Description ?? DBNull.Value,
                book.Active,
                book.Id
            });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM Books WHERE Id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
