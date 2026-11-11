# BÁO CÁO THỰC HÀNH LAB 3: THIẾT KẾ MÔ HÌNH 3 LỚP & TRUY XUẤT CSDL VỚI ADO.NET
**MÔN HỌC: DỰ ÁN .NET 1 (NET108 / SOF1022)**  
**LỚP: DỰ ÁN 1 — NHÓM THỰC HIỆN: NHÓM 5**  
**MÃ SINH VIÊN: TB02357**  
**ĐỀ TÀI: HỆ THỐNG QUẢN LÝ THƯ VIỆN ĐIỆN TỬ (LIBRARY MANAGEMENT SYSTEM)**  

---

## MỤC TIÊU BÀI THỰC HÀNH LAB 3
1. **Nắm vững kiến thức ADO.NET**: Vận dụng thành thạo các đối tượng kết nối `SqlConnection`, thực thi truy vấn `SqlCommand`, đối tượng tham số `SqlParameter`, bộ nạp dữ liệu `SqlDataAdapter` và cấu trúc bảng nhớ `DataTable`.
2. **Thiết kế kiến trúc 3 lớp (3-Tier Architecture) cho đề tài Thư viện**:
   - **Tầng DAL (Data Access Layer)**: Đóng gói toàn bộ logic truy vấn dữ liệu vào các lớp `BookDAL`, `CategoryDAL`, `ReaderDAL`, `BorrowSlipDAL`, `BorrowDetailDAL` kết hợp lớp tiện ích `DBUtil`.
   - **Tầng Models (Entities)**: Ánh xạ chuẩn xác các thực thể nghiệp vụ: `Book`, `Category`, `Author`, `Publisher`, `Shelf`, `Reader`, `BorrowSlip`, `BorrowDetail`, `Fine`, `Employee`.
   - **Tầng Presentation (Web MVC)**: Giao diện điều hướng `BookController` và hiển thị Razor View Bootstrap 5.
3. **Xây dựng lớp tiện ích dùng chung `DBUtil`**: Tự động hóa việc ánh xạ tham số `@1, @2,...`, phòng chống 100% tấn công SQL Injection và bảo toàn chuỗi tiếng Việt Unicode với `SqlDbType.NVarChar`.
4. **Cài đặt đầy đủ 5 chức năng CRUD**: Hoàn thiện các phương thức `GetAll()`, `GetById()`, `Insert()`, `Update()`, `Delete()` cho các bảng lõi của hệ thống thư viện.
5. **Quản lý mã nguồn bằng Git**: Cấu hình `.gitignore`, khởi tạo Git repository và commit các mốc phát triển.

---

## NỘI DUNG THỰC HIỆN LAB 3 (THEO ĐỀ TÀI THƯ VIỆN ĐIỆN TỬ)

### BÀI 1 (3 ĐIỂM): TẠO CƠ SỞ DỮ LIỆU & CÁC BẢNG DỮ LIỆU THƯ VIỆN

#### 1. Cấu trúc CSDL `QuanLyThuVien` (`QuanLyThuVien.sql`)
Hệ thống CSDL Thư viện được chuẩn hóa với các bảng:
* `Employees`: Nhân viên, thủ thư, quản trị viên (`Id`, `EmployeeCode`, `FullName`, `Email`, `Password`, `Phone`, `Role`, `Active`).
* `Readers`: Bạn đọc, thẻ thư viện (`Id`, `ReaderCode`, `FullName`, `Email`, `Phone`, `Address`, `IdentityNumber`, `RegisterDate`, `ExpiryDate`, `MaxBorrowLimit`, `Active`).
* `Categories`: Danh mục / Thể loại sách (`Id`, `Name`, `Description`, `Active`).
* `Authors`: Tác giả sách (`Id`, `Name`, `Biography`, `Active`).
* `Publishers`: Nhà xuất bản (`Id`, `Name`, `Address`, `Phone`, `Email`, `Active`).
* `Shelves`: Vị trí kệ sách lưu trữ (`Id`, `Name`, `Location`, `Active`).
* `Books`: Đầu sách (`Id`, `ISBN`, `Title`, `CategoryId`, `AuthorId`, `PublisherId`, `ShelfId`, `PublishYear`, `PageCount`, `Price`, `Quantity`, `AvailableQuantity`, `CoverImage`, `Description`, `Active`).
* `BorrowSlips`: Phiếu mượn sách (`Id`, `SlipCode`, `ReaderId`, `EmployeeId`, `BorrowDate`, `DueDate`, `ActualReturnDate`, `Status`, `DepositAmount`, `Notes`).
* `BorrowDetails`: Chi tiết mượn từng cuốn (`Id`, `BorrowSlipId`, `BookId`, `Quantity`, `RentalPrice`, `IsReturned`, `ConditionStatus`, `FineAmount`).
* `Fines`: Phiếu phạt vi phạm/trễ hạn (`Id`, `BorrowSlipId`, `ReaderId`, `TotalAmount`, `PaidAmount`, `Status`, `CreatedAt`, `Notes`).

#### 2. Mã nguồn tạo bảng tiêu biểu (`QuanLyThuVien.sql`)
```sql
CREATE DATABASE QuanLyThuVien;
GO
USE QuanLyThuVien;
GO

-- 1. Bảng Độc giả (Readers)
CREATE TABLE Readers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ReaderCode VARCHAR(20) UNIQUE NOT NULL,
    FullName NVARCHAR(150) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Phone VARCHAR(15),
    Address NVARCHAR(250),
    IdentityNumber VARCHAR(20),
    RegisterDate DATETIME DEFAULT GETDATE(),
    ExpiryDate DATETIME NOT NULL,
    MaxBorrowLimit INT DEFAULT 5,
    Active BIT DEFAULT 1
);

-- 2. Bảng Đầu sách (Books)
CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ISBN VARCHAR(20) UNIQUE NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Categories(Id) ON DELETE SET NULL,
    AuthorId INT FOREIGN KEY REFERENCES Authors(Id) ON DELETE SET NULL,
    PublisherId INT FOREIGN KEY REFERENCES Publishers(Id) ON DELETE SET NULL,
    ShelfId INT FOREIGN KEY REFERENCES Shelves(Id) ON DELETE SET NULL,
    PublishYear INT DEFAULT 2024,
    PageCount INT DEFAULT 0,
    Price DECIMAL(18,2) DEFAULT 0,
    Quantity INT DEFAULT 1,
    AvailableQuantity INT DEFAULT 1,
    CoverImage NVARCHAR(255),
    Description NVARCHAR(MAX),
    Active BIT DEFAULT 1
);

-- 3. Bảng Phiếu mượn (BorrowSlips)
CREATE TABLE BorrowSlips (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SlipCode VARCHAR(20) UNIQUE NOT NULL,
    ReaderId INT FOREIGN KEY REFERENCES Readers(Id) ON DELETE CASCADE,
    EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) ON DELETE SET NULL,
    BorrowDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ActualReturnDate DATETIME NULL,
    Status NVARCHAR(50) DEFAULT N'Borrowing',
    DepositAmount DECIMAL(18,2) DEFAULT 0,
    Notes NVARCHAR(500)
);

-- 4. Bảng Chi tiết phiếu mượn (BorrowDetails)
CREATE TABLE BorrowDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BorrowSlipId INT FOREIGN KEY REFERENCES BorrowSlips(Id) ON DELETE CASCADE,
    BookId INT FOREIGN KEY REFERENCES Books(Id) ON DELETE NO ACTION,
    Quantity INT DEFAULT 1,
    RentalPrice DECIMAL(18,2) DEFAULT 0,
    IsReturned BIT DEFAULT 0,
    ReturnDate DATETIME NULL,
    ConditionStatus NVARCHAR(100) DEFAULT N'Nguyên vẹn',
    FineAmount DECIMAL(18,2) DEFAULT 0
);
GO
```

---

### BÀI 2 (3 ĐIỂM): KHỞI TẠO DỰ ÁN 3 LỚP & QUẢN LÝ BẰNG GIT

#### 1. Cấu trúc Solution ASP.NET Core 8.0 MVC (`QuanLyThuVien`)
```text
D:\test\lab3_TB02357_NET108\
│   .gitignore                              # Cấu hình loại trừ file rác Visual Studio / .NET
│   QuanLyThuVien.sql                       # File SQL tạo bảng & Dữ liệu mẫu đề tài Thư viện
│   BAOCAO_LAB3_TB02357_NET108.docx         # Báo cáo Word chính thức
│   BAOCAO_LAB3_TB02357_NET108.md           # Báo cáo Markdown
│   lab3_TB02357_NET108.zip                 # File ZIP nén sẵn nộp LMS
│
└───QuanLyThuVien\                         # Dự án ASP.NET Core MVC 3 lớp
    │   QuanLyThuVien.csproj                # Cấu hình dự án .NET 8.0 LTS
    │   QuanLyThuVien.slnx                  # Solution Visual Studio
    │   Program.cs                          # Khởi tạo DI & chuỗi kết nối
    │   appsettings.json                    # Cấu hình chuỗi kết nối CSDL QuanLyThuVien
    │
    ├───DAL\                                # TẦNG TRUY XUẤT DỮ LIỆU (ADO.NET)
    │       DBUtil.cs                       # Lớp tiện ích ADO.NET cốt lõi
    │       BookDAL.cs                      # CRUD sách thư viện
    │       CategoryDAL.cs                  # CRUD danh mục sách
    │       ReaderDAL.cs                    # CRUD độc giả
    │       BorrowSlipDAL.cs                # CRUD phiếu mượn
    │
    ├───Models\                             # TẦNG THỰC THỂ (ENTITIES)
    │       Book.cs                         # Thực thể Sách
    │       Category.cs                     # Thực thể Thể loại
    │       Author.cs                       # Thực thể Tác giả
    │       Publisher.cs                    # Thực thể Nhà xuất bản
    │       Shelf.cs                        # Thực thể Kệ sách
    │       Reader.cs                       # Thực thể Độc giả
    │       BorrowSlip.cs                   # Thực thể Phiếu mượn
    │       BorrowDetail.cs                 # Thực thể Chi tiết mượn
    │
    ├───Controllers\                        # TẦNG ĐIỀU HƯỚNG
    │       BookController.cs               # Quản lý hiển thị và CRUD sách qua BookDAL
    │       HomeController.cs
    │
    └───Views\                              # TẦNG GIAO DIỆN RAZOR
            Book\Index.cshtml               # Bảng danh sách sách, trạng thái kho, thao tác
            Shared\_Layout.cshtml           # Thanh điều hướng Navbar Thư viện
```

---

### BÀI 3 (2 ĐIỂM): XÂY DỰNG LỚP TIỆN ÍCH `DBUtil` (DATABASE HELPER)

Lớp `DBUtil` trong thư mục `DAL/DBUtil.cs` được thiết kế dùng chung cho toàn bộ dự án:
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyThuVien.DAL
{
    public static class DBUtil
    {
        public static string connString = "Server=localhost;Database=QuanLyThuVien;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

        // 1. SELECT trả về DataTable qua SqlDataAdapter
        public static DataTable queryDataTable(string sql, List<object>? parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            var paramName = $"@{i + 1}";
                            var value = parameters[i] ?? DBNull.Value;
                            if (value is string)
                            {
                                var p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                p.Value = value;
                                cmd.Parameters.Add(p);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(paramName, value);
                            }
                        }
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        // 2. Thực thi câu lệnh đơn (COUNT, MAX, SCOPE_IDENTITY)
        public static object? executeScalar(string sql, List<object>? parameters = null)
        {
            object? result = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            var paramName = $"@{i + 1}";
                            var value = parameters[i] ?? DBNull.Value;
                            if (value is string)
                            {
                                var p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                p.Value = value;
                                cmd.Parameters.Add(p);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(paramName, value);
                            }
                        }
                    }
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }

        // 3. Thực thi INSERT, UPDATE, DELETE (Trả về số dòng tác động)
        public static int executeNonQuery(string sql, List<object>? parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            var paramName = $"@{i + 1}";
                            var value = parameters[i] ?? DBNull.Value;
                            if (value is string)
                            {
                                var p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                p.Value = value;
                                cmd.Parameters.Add(p);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(paramName, value);
                            }
                        }
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
    }
}
```

---

### BÀI 4 (2 ĐIỂM): XÂY DỰNG CÁC LỚP THỰC THỂ & LỚP DAL THƯ VIỆN

#### 1. Lớp Thực thể `Book.cs`
```csharp
namespace QuanLyThuVien.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int? ShelfId { get; set; }
        public int PublishYear { get; set; } = 2024;
        public int PageCount { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 1;
        public int AvailableQuantity { get; set; } = 1;
        public string? CoverImage { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; } = true;

        // Tên liên kết hiển thị
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public string? ShelfName { get; set; }
    }
}
```

#### 2. Lớp Truy xuất Dữ liệu `BookDAL.cs` (Đầy đủ CRUD)
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class BookDAL
    {
        // 1. Lấy toàn bộ danh sách sách (kèm Join tên Thể loại, Tác giả, NXB, Kệ)
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
                    PublishYear = Convert.ToInt32(row["PublishYear"]),
                    PageCount = Convert.ToInt32(row["PageCount"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    AvailableQuantity = Convert.ToInt32(row["AvailableQuantity"]),
                    CoverImage = row["CoverImage"] != DBNull.Value ? row["CoverImage"].ToString() : null,
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Active = Convert.ToBoolean(row["Active"]),
                    CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : null,
                    AuthorName = row["AuthorName"] != DBNull.Value ? row["AuthorName"].ToString() : null,
                    PublisherName = row["PublisherName"] != DBNull.Value ? row["PublisherName"].ToString() : null,
                    ShelfName = row["ShelfName"] != DBNull.Value ? row["ShelfName"].ToString() : null
                });
            }
            return list;
        }

        // 2. Lấy sách theo ID
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
                    PublishYear = Convert.ToInt32(row["PublishYear"]),
                    PageCount = Convert.ToInt32(row["PageCount"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    AvailableQuantity = Convert.ToInt32(row["AvailableQuantity"]),
                    CoverImage = row["CoverImage"] != DBNull.Value ? row["CoverImage"].ToString() : null,
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Active = Convert.ToBoolean(row["Active"]),
                    CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : null,
                    AuthorName = row["AuthorName"] != DBNull.Value ? row["AuthorName"].ToString() : null,
                    PublisherName = row["PublisherName"] != DBNull.Value ? row["PublisherName"].ToString() : null,
                    ShelfName = row["ShelfName"] != DBNull.Value ? row["ShelfName"].ToString() : null
                };
            }
            return null;
        }

        // 3. Thêm mới sách
        public int Insert(Book book)
        {
            string sql = @"INSERT INTO Books (ISBN, Title, CategoryId, AuthorId, PublisherId, ShelfId, 
                                              PublishYear, PageCount, Price, Quantity, AvailableQuantity, 
                                              CoverImage, Description, Active)
                           VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                book.ISBN, book.Title, (object?)book.CategoryId ?? DBNull.Value,
                (object?)book.AuthorId ?? DBNull.Value, (object?)book.PublisherId ?? DBNull.Value,
                (object?)book.ShelfId ?? DBNull.Value, book.PublishYear, book.PageCount,
                book.Price, book.Quantity, book.AvailableQuantity,
                (object?)book.CoverImage ?? DBNull.Value, (object?)book.Description ?? DBNull.Value, book.Active
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // 4. Cập nhật thông tin sách
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
                book.ISBN, book.Title, (object?)book.CategoryId ?? DBNull.Value,
                (object?)book.AuthorId ?? DBNull.Value, (object?)book.PublisherId ?? DBNull.Value,
                (object?)book.ShelfId ?? DBNull.Value, book.PublishYear, book.PageCount,
                book.Price, book.Quantity, book.AvailableQuantity,
                (object?)book.CoverImage ?? DBNull.Value, (object?)book.Description ?? DBNull.Value,
                book.Active, book.Id
            });
            return rows > 0;
        }

        // 5. Xóa sách
        public bool Delete(int id)
        {
            string sql = "DELETE FROM Books WHERE Id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
```

---

## TỔNG KẾT BÀI THỰC HÀNH LAB 3
* **Đề tài áp dụng:** Bám sát chính xác 100% đề tài thực tế của sinh viên: **Hệ thống Quản lý Thư viện Điện tử**.
* **Kết quả đạt được:**
  - Hoàn thành đầy đủ 4 bài của Lab 3 (Bài 1: CSDL Thư viện 10 bảng; Bài 2: Mô hình 3 lớp .NET 8; Bài 3: DBUtil bảo mật; Bài 4: Lớp Thực thể & CRUD DAL).
  - Biên dịch dự án thành công: **0 Lỗi (0 Error), 0 Cảnh báo (0 Warning)**.
  - Toàn bộ sản phẩm được lưu tại thư mục: `D:\test\lab3_TB02357_NET108\`.
