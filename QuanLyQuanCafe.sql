-- ==============================================================================
-- BÀI THỰC HÀNH LAB 3: THIẾT KẾ MÔ HÌNH 3 LỚP & TRUY XUẤT CSDL VỚI ADO.NET
-- MÔN HỌC: DỰ ÁN .NET 1 (NET108)
-- SINH VIÊN: TB02357 | NHÓM: NHÓM 5
-- BÀI 1: TẠO CƠ SỞ DỮ LIỆU & BẢNG DỮ LIỆU (QUẢN LÝ QUÁN CAFE)
-- ==============================================================================

USE master;
GO

-- 1. Tạo Database nếu chưa tồn tại
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyQuanCafe')
BEGIN
    CREATE DATABASE QuanLyQuanCafe;
    PRINT N'Đã tạo thành công cơ sở dữ liệu QuanLyQuanCafe';
END
GO

USE QuanLyQuanCafe;
GO

-- Xóa các bảng cũ nếu đã tồn tại để tạo mới sạch sẽ
IF OBJECT_ID('dbo.bill_details', 'U') IS NOT NULL DROP TABLE dbo.bill_details;
IF OBJECT_ID('dbo.bills', 'U') IS NOT NULL DROP TABLE dbo.bills;
IF OBJECT_ID('dbo.cards', 'U') IS NOT NULL DROP TABLE dbo.cards;
IF OBJECT_ID('dbo.users', 'U') IS NOT NULL DROP TABLE dbo.users;
IF OBJECT_ID('dbo.drinks', 'U') IS NOT NULL DROP TABLE dbo.drinks;
IF OBJECT_ID('dbo.categories', 'U') IS NOT NULL DROP TABLE dbo.categories;
GO

-- 2. Tạo bảng danh mục đồ uống (categories)
CREATE TABLE categories (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(250) NOT NULL,
    active BIT DEFAULT 1
);
GO

-- 3. Tạo bảng đồ uống (drinks)
CREATE TABLE drinks (
    id INT PRIMARY KEY IDENTITY(1,1),
    category_id INT FOREIGN KEY REFERENCES categories(id) ON DELETE SET NULL,
    name NVARCHAR(250) NOT NULL,
    price INT NOT NULL DEFAULT 0,
    image NVARCHAR(250),
    description NVARCHAR(MAX),
    active BIT DEFAULT 1
);
GO

-- 4. Tạo bảng người dùng / nhân viên (users)
CREATE TABLE users (
    id INT PRIMARY KEY IDENTITY(1,1),
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    full_name NVARCHAR(60) NOT NULL,
    phone VARCHAR(10),
    role BIT DEFAULT 0, -- 0: Nhân viên (Staff), 1: Quản lý (Admin)
    active BIT DEFAULT 1
);
GO

-- 5. Tạo bảng thẻ bàn / thẻ rung tự phục vụ (cards)
CREATE TABLE cards (
    id INT PRIMARY KEY IDENTITY(1,1),
    code INT NOT NULL UNIQUE,
    status BIT DEFAULT 1 -- 1: Đang sẵn sàng, 0: Đang có khách giữ
);
GO

-- 6. Tạo bảng hóa đơn (bills)
CREATE TABLE bills (
    id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES users(id) ON DELETE SET NULL,
    card_id INT FOREIGN KEY REFERENCES cards(id) ON DELETE SET NULL,
    code VARCHAR(10) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    total INT DEFAULT 0,
    status VARCHAR(10) DEFAULT 'PAID' -- PAID: Đã thanh toán, PENDING: Chờ, CANCEL: Hủy
);
GO

-- 7. Tạo bảng chi tiết hóa đơn (bill_details)
CREATE TABLE bill_details (
    id INT PRIMARY KEY IDENTITY(1,1),
    bill_id INT FOREIGN KEY REFERENCES bills(id) ON DELETE CASCADE,
    drink_id INT FOREIGN KEY REFERENCES drinks(id) ON DELETE NO ACTION,
    quantity INT NOT NULL DEFAULT 1,
    price INT NOT NULL DEFAULT 0
);
GO

-- ==============================================================================
-- THÊM DỮ LIỆU MẪU BAN ĐẦU (SEED DATA)
-- ==============================================================================

-- 1. Thêm danh mục categories
INSERT INTO categories (name, active) VALUES 
(N'Cà phê truyền thống', 1),
(N'Cà phê pha máy (Espresso)', 1),
(N'Trà trái cây & Trà sữa', 1),
(N'Đá xay & Sinh tố', 1),
(N'Bánh ngọt & Đồ ăn nhẹ', 1);

-- 2. Thêm đồ uống drinks
INSERT INTO drinks (category_id, name, price, image, description, active) VALUES
(1, N'Cà phê đen đá', 25000, 'caphe_den.jpg', N'Cà phê Robusta Đắk Lắk đậm đà nguyên chất', 1),
(1, N'Cà phê sữa đá', 28000, 'caphe_sua.jpg', N'Cà phê phin truyền thống hòa quyện cùng sữa đặc béo ngậy', 1),
(1, N'Bạc xỉu Sài Gòn', 30000, 'bac_xiu.jpg', N'Nhiều sữa ít cà phê, béo ngậy thơm ngon', 1),
(2, N'Americano đá', 35000, 'americano.jpg', N'Espresso pha loãng với nước thanh mát, ít chua', 1),
(2, N'Cà phê Latte nóng', 45000, 'latte.jpg', N'Espresso kết hợp sữa tươi đánh nóng nghệ thuật', 1),
(2, N'Cappuccino', 45000, 'cappuccino.jpg', N'Lớp bọt sữa dày mịn cùng bột ca cao phủ bề mặt', 1),
(3, N'Trà đào cam sả', 38000, 'tra_dao.jpg', N'Hương sả thanh mát kết hợp lát đào giòn ngọt', 1),
(3, N'Trà vải hoa hồng', 40000, 'tra_vai.jpg', N'Trà đen ủ lạnh kết hợp trái vải tươi mọng nước', 1),
(4, N'Matcha đá xay kem cheese', 48000, 'matcha_ice.jpg', N'Bột trà xanh Nhật Bản xay nhuyễn phủ kem phô mai', 1),
(5, N'Bánh Tiramisu', 35000, 'tiramisu.jpg', N'Bánh ngọt vị cà phê và cacao mềm mịn chuẩn Ý', 1);

-- 3. Thêm tài khoản người dùng users (mật khẩu đã băm giả lập SHA256)
INSERT INTO users (email, password, full_name, phone, role, active) VALUES
('admin@cafe.vn', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', N'Quản Lý - Nhóm 5', '0912345678', 1, 1),
('nv01@cafe.vn', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', N'Trần Văn Nhân', '0987654321', 0, 1),
('nv02@cafe.vn', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', N'Lê Thị Viên', '0905123456', 0, 1);

-- 4. Thêm thẻ bàn / thẻ rung cards
INSERT INTO cards (code, status) VALUES
(101, 1),
(102, 1),
(103, 0),
(104, 1),
(105, 1);

-- 5. Thêm hóa đơn bills
INSERT INTO bills (user_id, card_id, code, created_at, total, status) VALUES
(2, 1, 'HD0001', DATEADD(HOUR, -2, GETDATE()), 53000, 'PAID'),
(2, 2, 'HD0002', DATEADD(HOUR, -1, GETDATE()), 73000, 'PAID'),
(3, 3, 'HD0003', GETDATE(), 83000, 'PENDING');

-- 6. Thêm chi tiết hóa đơn bill_details
INSERT INTO bill_details (bill_id, drink_id, quantity, price) VALUES
(1, 1, 1, 25000), -- 1 cà phê đen
(1, 2, 1, 28000), -- 1 cà phê sữa
(2, 3, 1, 30000), -- 1 bạc xỉu
(2, 7, 1, 38000), -- 1 trà đào
(3, 9, 1, 48000), -- 1 matcha đá xay
(3, 10, 1, 35000); -- 1 tiramisu
GO

-- Kiểm tra truy vấn xác nhận
SELECT 'categories' AS TableName, COUNT(*) AS TotalRows FROM categories
UNION ALL
SELECT 'drinks', COUNT(*) FROM drinks
UNION ALL
SELECT 'users', COUNT(*) FROM users
UNION ALL
SELECT 'cards', COUNT(*) FROM cards
UNION ALL
SELECT 'bills', COUNT(*) FROM bills
UNION ALL
SELECT 'bill_details', COUNT(*) FROM bill_details;
GO
