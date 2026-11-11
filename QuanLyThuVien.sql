-- ==============================================================================
-- BÀI THỰC HÀNH LAB 3: THIẾT KẾ MÔ HÌNH 3 LỚP & TRUY XUẤT CSDL VỚI ADO.NET
-- MÔN HỌC: DỰ ÁN .NET 1 (NET108)
-- SINH VIÊN: TB02357 | NHÓM: NHÓM 5
-- ĐỀ TÀI: HỆ THỐNG QUẢN LÝ THƯ VIỆN ĐIỆN TỬ (LIBRARY MANAGEMENT SYSTEM)
-- BÀI 1: TẠO CƠ SỞ DỮ LIỆU & BẢNG DỮ LIỆU CHUẨN ĐỀ TÀI THỰC TẾ
-- ==============================================================================

USE master;
GO

-- 1. Tạo Database nếu chưa tồn tại
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyThuVien')
BEGIN
    CREATE DATABASE QuanLyThuVien;
    PRINT N'Đã tạo thành công cơ sở dữ liệu QuanLyThuVien';
END
GO

USE QuanLyThuVien;
GO

-- Xóa các bảng cũ nếu đã tồn tại theo thứ tự ràng buộc khóa ngoại
IF OBJECT_ID('dbo.FineDetails', 'U') IS NOT NULL DROP TABLE dbo.FineDetails;
IF OBJECT_ID('dbo.Fines', 'U') IS NOT NULL DROP TABLE dbo.Fines;
IF OBJECT_ID('dbo.BorrowDetails', 'U') IS NOT NULL DROP TABLE dbo.BorrowDetails;
IF OBJECT_ID('dbo.BorrowSlips', 'U') IS NOT NULL DROP TABLE dbo.BorrowSlips;
IF OBJECT_ID('dbo.Reservations', 'U') IS NOT NULL DROP TABLE dbo.Reservations;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Shelves', 'U') IS NOT NULL DROP TABLE dbo.Shelves;
IF OBJECT_ID('dbo.Publishers', 'U') IS NOT NULL DROP TABLE dbo.Publishers;
IF OBJECT_ID('dbo.Authors', 'U') IS NOT NULL DROP TABLE dbo.Authors;
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID('dbo.Readers', 'U') IS NOT NULL DROP TABLE dbo.Readers;
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL DROP TABLE dbo.Employees;
GO

-- 1. Bảng Nhân viên / Quản lý thư viện (Employees)
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeCode VARCHAR(20) UNIQUE NOT NULL,
    FullName NVARCHAR(150) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Phone VARCHAR(15),
    Role NVARCHAR(50) DEFAULT N'Librarian', -- Admin, Librarian, Staff
    Active BIT DEFAULT 1
);
GO

-- 2. Bảng Độc giả / Bạn đọc (Readers)
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
GO

-- 3. Bảng Thể loại sách (Categories)
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500),
    Active BIT DEFAULT 1
);
GO

-- 4. Bảng Tác giả (Authors)
CREATE TABLE Authors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    Biography NVARCHAR(MAX),
    Active BIT DEFAULT 1
);
GO

-- 5. Bảng Nhà xuất bản (Publishers)
CREATE TABLE Publishers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Address NVARCHAR(250),
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Active BIT DEFAULT 1
);
GO

-- 6. Bảng Kệ sách / Vị trí lưu kho (Shelves)
CREATE TABLE Shelves (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL, -- Kệ A1, Kệ B2...
    Location NVARCHAR(200),     -- Tầng 2, Dãy 3...
    Active BIT DEFAULT 1
);
GO

-- 7. Bảng Đầu sách (Books)
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
GO

-- 8. Bảng Phiếu mượn sách (BorrowSlips)
CREATE TABLE BorrowSlips (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SlipCode VARCHAR(20) UNIQUE NOT NULL,
    ReaderId INT FOREIGN KEY REFERENCES Readers(Id) ON DELETE CASCADE,
    EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) ON DELETE SET NULL,
    BorrowDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ActualReturnDate DATETIME NULL,
    Status NVARCHAR(50) DEFAULT N'Borrowing', -- Borrowing, Returned, Overdue, Cancelled
    DepositAmount DECIMAL(18,2) DEFAULT 0,
    Notes NVARCHAR(500)
);
GO

-- 9. Bảng Chi tiết phiếu mượn sách (BorrowDetails)
CREATE TABLE BorrowDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BorrowSlipId INT FOREIGN KEY REFERENCES BorrowSlips(Id) ON DELETE CASCADE,
    BookId INT FOREIGN KEY REFERENCES Books(Id) ON DELETE NO ACTION,
    Quantity INT DEFAULT 1,
    RentalPrice DECIMAL(18,2) DEFAULT 0,
    IsReturned BIT DEFAULT 0,
    ReturnDate DATETIME NULL,
    ConditionStatus NVARCHAR(100) DEFAULT N'Nguyên vẹn', -- Nguyên vẹn, Rách bìa, Mất sách
    FineAmount DECIMAL(18,2) DEFAULT 0
);
GO

-- 10. Bảng Phiếu phạt vi phạm (Fines)
CREATE TABLE Fines (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BorrowSlipId INT FOREIGN KEY REFERENCES BorrowSlips(Id) ON DELETE CASCADE,
    ReaderId INT FOREIGN KEY REFERENCES Readers(Id) ON DELETE NO ACTION,
    TotalAmount DECIMAL(18,2) DEFAULT 0,
    PaidAmount DECIMAL(18,2) DEFAULT 0,
    Status NVARCHAR(50) DEFAULT N'Unpaid', -- Unpaid, Paid, PartiallyPaid
    CreatedAt DATETIME DEFAULT GETDATE(),
    Notes NVARCHAR(500)
);
GO

-- ==============================================================================
-- DỮ LIỆU MẪU BAN ĐẦU (SEED DATA CHO HỆ THỐNG THƯ VIỆN)
-- ==============================================================================

-- 1. Thêm Thể loại
INSERT INTO Categories (Name, Description, Active) VALUES
(N'Công nghệ thông tin', N'Sách lập trình, cơ sở dữ liệu, mạng máy tính, AI', 1),
(N'Kinh tế & Quản trị', N'Quản trị kinh doanh, tài chính, khởi nghiệp', 1),
(N'Văn học & Nghệ thuật', N'Tiểu thuyết, truyện ngắn, tác phẩm kinh điển', 1),
(N'Kỹ năng sống & Phát triển bản thân', N'Tư duy, giao tiếp, tâm lý học ứng dụng', 1),
(N'Khoa học cơ bản', N'Toán học, vật lý, thiên văn học', 1);

-- 2. Thêm Tác giả
INSERT INTO Authors (Name, Biography, Active) VALUES
(N'Robert C. Martin (Uncle Bob)', N'Chuyên gia kiến trúc phần mềm, tác giả Clean Code', 1),
(N'Andrew Hunt & David Thomas', N'Tác giả cuốn sách kinh điển The Pragmatic Programmer', 1),
(N'Dale Carnegie', N'Tác giả cuốn Đắc Nhân Tâm kinh điển mọi thời đại', 1),
(N'Nguyễn Nhật Ánh', N'Nhà văn nổi tiếng Việt Nam với nhiều tác phẩm tuổi học trò', 1),
(N'Yuval Noah Harari', N'Sử gia, tác giả Sapiens: Lược sử loài người', 1);

-- 3. Thêm Nhà xuất bản
INSERT INTO Publishers (Name, Address, Phone, Email, Active) VALUES
(N'NXB Thông tin và Truyền thông', N'Hà Nội', '02435770000', 'nxbtt@gmail.com', 1),
(N'NXB Trẻ', N'TP. Hồ Chí Minh', '02839316289', 'nxbtre@hcm.fpt.vn', 1),
(N'NXB Kim Đồng', N'Hà Nội', '02439434730', 'info@nxbkimdong.com.vn', 1),
(N'NXB Khoa học và Kỹ thuật', N'Hà Nội', '02438253164', 'nxbkhkt@hn.vnn.vn', 1);

-- 4. Thêm Kệ sách
INSERT INTO Shelves (Name, Location, Active) VALUES
(N'Kệ A1 - CNTT', N'Tầng 2 - Dãy A bên trái', 1),
(N'Kệ A2 - Lập trình .NET', N'Tầng 2 - Dãy A bên phải', 1),
(N'Kệ B1 - Kinh tế', N'Tầng 1 - Dãy B trung tâm', 1),
(N'Kệ C1 - Văn học', N'Tầng 1 - Dãy C', 1);

-- 5. Thêm Nhân viên
INSERT INTO Employees (EmployeeCode, FullName, Email, Password, Phone, Role, Active) VALUES
('NV001', N'Nguyễn Quản Trị', 'admin@thuvien.edu.vn', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', '0911223344', N'Admin', 1),
('NV002', N'Trần Thị Thủ Thư', 'thuthu01@thuvien.edu.vn', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '0988776655', N'Librarian', 1);

-- 6. Thêm Độc giả
INSERT INTO Readers (ReaderCode, FullName, Email, Password, Phone, Address, IdentityNumber, RegisterDate, ExpiryDate, MaxBorrowLimit, Active) VALUES
('DG001', N'Trần Minh Tuấn', 'tuan.tm@gmail.com', '123456', '0933112233', N'Cầu Giấy, Hà Nội', '001202001122', GETDATE(), DATEADD(YEAR, 1, GETDATE()), 5, 1),
('DG002', N'Lê Hoàng Yến', 'yen.lh@gmail.com', '123456', '0944223344', N'Bắc Từ Liêm, Hà Nội', '001202003344', GETDATE(), DATEADD(YEAR, 1, GETDATE()), 5, 1);

-- 7. Thêm Sách
INSERT INTO Books (ISBN, Title, CategoryId, AuthorId, PublisherId, ShelfId, PublishYear, PageCount, Price, Quantity, AvailableQuantity, CoverImage, Description, Active) VALUES
('9780132350884', N'Clean Code: A Handbook of Agile Software Craftsmanship', 1, 1, 1, 2, 2021, 464, 250000, 10, 8, 'cleancode.jpg', N'Cuốn sách kinh điển về kỹ nghệ viết mã sạch, tối ưu và dễ bảo trì.', 1),
('9780201616224', N'The Pragmatic Programmer: Your Journey To Mastery', 1, 2, 1, 2, 2022, 352, 280000, 8, 7, 'pragmatic.jpg', N'Hành trình từ lập trình viên nghiệp dư trở thành bậc thầy phát triển phần mềm.', 1),
('9786045882885', N'Đắc Nhân Tâm (How to Win Friends and Influence People)', 4, 3, 2, 3, 2023, 320, 95000, 15, 14, 'dacnhantam.jpg', N'Nghệ thuật thu phục lòng người và xây dựng mối quan hệ bền vững.', 1),
('9786042171120', N'Mắt Biếc', 3, 4, 3, 4, 2020, 300, 110000, 12, 11, 'matbiec.jpg', N'Tác phẩm văn học lãng mạn đầy cảm xúc của nhà văn Nguyễn Nhật Ánh.', 1),
('9780062316097', N'Sapiens: Lược Sử Loài Người', 5, 5, 4, 3, 2022, 512, 185000, 10, 10, 'sapiens.jpg', N'Khám phá toàn cảnh lịch sử tiến hóa và văn minh nhân loại từ thuở sơ khai.', 1);

-- 8. Thêm Phiếu mượn
INSERT INTO BorrowSlips (SlipCode, ReaderId, EmployeeId, BorrowDate, DueDate, Status, DepositAmount, Notes) VALUES
('PM0001', 1, 2, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, 9, GETDATE()), N'Borrowing', 100000, N'Mượn 2 cuốn sách CNTT kỳ 1'),
('PM0002', 2, 2, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, 4, GETDATE()), N'Borrowing', 50000, N'Mượn sách văn học');

-- 9. Thêm Chi tiết phiếu mượn
INSERT INTO BorrowDetails (BorrowSlipId, BookId, Quantity, RentalPrice, IsReturned, ConditionStatus, FineAmount) VALUES
(1, 1, 1, 10000, 0, N'Nguyên vẹn', 0),
(1, 2, 1, 10000, 0, N'Nguyên vẹn', 0),
(2, 4, 1, 5000, 0, N'Nguyên vẹn', 0);
GO

-- Kiểm tra kết quả
SELECT 'Categories' AS TableName, COUNT(*) AS Total FROM Categories
UNION ALL SELECT 'Authors', COUNT(*) FROM Authors
UNION ALL SELECT 'Publishers', COUNT(*) FROM Publishers
UNION ALL SELECT 'Shelves', COUNT(*) FROM Shelves
UNION ALL SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL SELECT 'Readers', COUNT(*) FROM Readers
UNION ALL SELECT 'Books', COUNT(*) FROM Books
UNION ALL SELECT 'BorrowSlips', COUNT(*) FROM BorrowSlips
UNION ALL SELECT 'BorrowDetails', COUNT(*) FROM BorrowDetails;
GO
