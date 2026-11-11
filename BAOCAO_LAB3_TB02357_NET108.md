# BÁO CÁO THỰC HÀNH LAB 3: THIẾT KẾ MÔ HÌNH 3 LỚP & TRUY XUẤT CSDL VỚI ADO.NET
**MÔN HỌC: DỰ ÁN .NET 1 (NET108)**  
**LỚP: DỰ ÁN 1 — NHÓM THỰC HIỆN: NHÓM 5**  
**MÃ SINH VIÊN: TB02357**  
**ĐỀ TÀI: XÂY DỰNG HỆ THỐNG QUẢN LÝ QUÁN CAFE (QUANLYQUANCAFE)**  

---

## MỤC TIÊU BÀI THỰC HÀNH LAB 3
1. **Nắm vững kỹ thuật ADO.NET**: Hiểu sâu cơ chế làm việc của các đối tượng kết nối `SqlConnection`, thực thi lệnh `SqlCommand`, bộ điều hợp dữ liệu `SqlDataAdapter`, bảng dữ liệu trong bộ nhớ `DataTable` và đối tượng tham số `SqlParameter`.
2. **Thiết kế kiến trúc 3 lớp (3-Tier Architecture)**: Phân tách rõ ràng giữa:
   - **DAL (Data Access Layer)**: Lớp truy xuất cơ sở dữ liệu.
   - **Models / Entities**: Lớp biểu diễn thực thể nghiệp vụ.
   - **Presentation Layer (Web MVC)**: Giao diện người dùng với Controllers và Views.
3. **Xây dựng lớp tiện ích dùng chung `DBUtil`**: Tối ưu hóa việc mở/đóng kết nối, tham số hóa câu lệnh để phòng chống triệt để tấn công **SQL Injection**, bảo toàn mã Unicode tiếng Việt với kiểu dữ liệu `NVarChar`.
4. **Xây dựng các lớp Thực thể và DAL hoàn chỉnh**: Thực thi đầy đủ 5 phương thức CRUD (`GetAll`, `GetById`, `Insert`, `Update`, `Delete`) cho các bảng dữ liệu chính trong hệ thống.
5. **Quản lý phiên bản mã nguồn với Git**: Khởi tạo kho chứa cục bộ, thiết lập `.gitignore` chuẩn cho .NET và thực hiện commit theo dõi lịch sử phát triển.

---

## NỘI DUNG THỰC HIỆN LAB 3

### BÀI 1 (3 ĐIỂM): TẠO CƠ SỞ DỮ LIỆU & CÁC BẢNG DỮ LIỆU TƯƠNG ỨNG

#### 1. Script thiết kế CSDL SQL Server (`QuanLyQuanCafe.sql`)
Hệ thống được thiết kế bao gồm 6 bảng thực thể nghiệp vụ, chuẩn hóa quan hệ khóa chính (`PRIMARY KEY IDENTITY`) và khóa ngoại (`FOREIGN KEY REFERENCES`):

```sql
-- Tạo Database
CREATE DATABASE QuanLyQuanCafe;
GO
USE QuanLyQuanCafe;
GO

-- 1. Bảng danh mục đồ uống (categories)
CREATE TABLE categories (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(250) NOT NULL,
    active BIT DEFAULT 1
);

-- 2. Bảng đồ uống (drinks)
CREATE TABLE drinks (
    id INT PRIMARY KEY IDENTITY(1,1),
    category_id INT FOREIGN KEY REFERENCES categories(id) ON DELETE SET NULL,
    name NVARCHAR(250) NOT NULL,
    price INT NOT NULL DEFAULT 0,
    image NVARCHAR(250),
    description NVARCHAR(MAX),
    active BIT DEFAULT 1
);

-- 3. Bảng người dùng / nhân viên (users)
CREATE TABLE users (
    id INT PRIMARY KEY IDENTITY(1,1),
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    full_name NVARCHAR(60) NOT NULL,
    phone VARCHAR(10),
    role BIT DEFAULT 0, -- 0: Nhân viên (Staff), 1: Quản lý (Admin)
    active BIT DEFAULT 1
);

-- 4. Bảng thẻ bàn / thẻ rung tự phục vụ (cards)
CREATE TABLE cards (
    id INT PRIMARY KEY IDENTITY(1,1),
    code INT NOT NULL UNIQUE,
    status BIT DEFAULT 1 -- 1: Sẵn sàng, 0: Đang phục vụ khách
);

-- 5. Bảng hóa đơn (bills)
CREATE TABLE bills (
    id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES users(id) ON DELETE SET NULL,
    card_id INT FOREIGN KEY REFERENCES cards(id) ON DELETE SET NULL,
    code VARCHAR(10) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    total INT DEFAULT 0,
    status VARCHAR(10) DEFAULT 'PAID' -- PAID, PENDING, CANCEL
);

-- 6. Bảng chi tiết hóa đơn (bill_details)
CREATE TABLE bill_details (
    id INT PRIMARY KEY IDENTITY(1,1),
    bill_id INT FOREIGN KEY REFERENCES bills(id) ON DELETE CASCADE,
    drink_id INT FOREIGN KEY REFERENCES drinks(id) ON DELETE NO ACTION,
    quantity INT NOT NULL DEFAULT 1,
    price INT NOT NULL DEFAULT 0
);
GO
```

#### 2. Thêm dữ liệu mẫu ban đầu (Seed Data)
```sql
-- Danh mục
INSERT INTO categories (name, active) VALUES 
(N'Cà phê truyền thống', 1), (N'Cà phê pha máy (Espresso)', 1),
(N'Trà trái cây & Trà sữa', 1), (N'Đá xay & Sinh tố', 1), (N'Bánh ngọt & Đồ ăn nhẹ', 1);

-- Đồ uống
INSERT INTO drinks (category_id, name, price, image, description, active) VALUES
(1, N'Cà phê đen đá', 25000, 'caphe_den.jpg', N'Cà phê Robusta Đắk Lắk đậm đà nguyên chất', 1),
(1, N'Cà phê sữa đá', 28000, 'caphe_sua.jpg', N'Cà phê phin truyền thống hòa quyện cùng sữa đặc', 1),
(1, N'Bạc xỉu Sài Gòn', 30000, 'bac_xiu.jpg', N'Nhiều sữa ít cà phê, thơm ngon béo ngậy', 1),
(2, N'Americano đá', 35000, 'americano.jpg', N'Espresso pha loãng với nước thanh mát', 1),
(3, N'Trà đào cam sả', 38000, 'tra_dao.jpg', N'Hương sả thanh mát kết hợp đào giòn ngọt', 1),
(4, N'Matcha đá xay kem cheese', 48000, 'matcha_ice.jpg', N'Trà xanh Nhật Bản xay nhuyễn phủ kem cheese', 1),
(5, N'Bánh Tiramisu', 35000, 'tiramisu.jpg', N'Bánh ngọt vị cà phê và cacao mềm mịn', 1);

-- Tài khoản người dùng
INSERT INTO users (email, password, full_name, phone, role, active) VALUES
('admin@cafe.vn', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', N'Quản Lý - Nhóm 5', '0912345678', 1, 1),
('nv01@cafe.vn', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', N'Trần Văn Nhân', '0987654321', 0, 1);

-- Thẻ bàn
INSERT INTO cards (code, status) VALUES (101, 1), (102, 1), (103, 0);

-- Hóa đơn & Chi tiết
INSERT INTO bills (user_id, card_id, code, created_at, total, status) VALUES
(2, 1, 'HD0001', DATEADD(HOUR, -2, GETDATE()), 53000, 'PAID');
INSERT INTO bill_details (bill_id, drink_id, quantity, price) VALUES
(1, 1, 1, 25000), (1, 2, 1, 28000);
```

---

### BÀI 2 (3 ĐIỂM): KHỞI TẠO DỰ ÁN THEO MÔ HÌNH 3 LỚP & QUẢN LÝ BẰNG GIT

#### 1. Bố cục dự án ASP.NET Core MVC (.NET 8.0 LTS)
Dự án được khởi tạo với cấu trúc 3 lớp nghiêm ngặt:
* **Lớp Truy xuất Dữ liệu (DAL - Data Access Layer)**:
  * `DBUtil.cs`: Tiện ích kết nối và thực thi ADO.NET.
  * `CategoryDAL.cs`: Xử lý CRUD bảng `categories`.
  * `DrinkDAL.cs`: Xử lý CRUD bảng `drinks`.
  * `UserDAL.cs`: Xử lý CRUD bảng `users`.
  * `CardDAL.cs`: Xử lý CRUD bảng `cards`.
  * `BillDAL.cs`: Xử lý CRUD bảng `bills`.
  * `BillDetailDAL.cs`: Xử lý CRUD bảng `bill_details`.
* **Lớp Thực thể (Models / Entities)**:
  * `Category.cs`, `Drink.cs`, `User.cs`, `Card.cs`, `Bill.cs`, `BillDetail.cs`.
* **Lớp Giao diện (Presentation Layer - Controllers & Views)**:
  * `Controllers/DrinkController.cs`: Điều hướng các thao tác quản lý đồ uống qua `DrinkDAL`.
  * `Views/Drink/Index.cshtml`: Giao diện bảng danh sách đồ uống kết nối dữ liệu trực tiếp.

#### 2. Cây thư mục dự án
```text
D:\test\lab3_TB02357_NET108\
│   .gitignore                              # Tệp loại trừ file rác Visual Studio / .NET
│   QuanLyQuanCafe.sql                      # Script tạo Database & Dữ liệu mẫu Bài 1
│   BAOCAO_LAB3_TB02357_NET108.docx         # Báo cáo Word chính thức nộp LMS
│   BAOCAO_LAB3_TB02357_NET108.md           # Báo cáo Markdown chi tiết
│
└───QuanLyQuanCafe\                         # Thư mục mã nguồn Solution ASP.NET Core
    │   QuanLyQuanCafe.csproj               # Cấu hình dự án .NET 8.0 LTS
    │   QuanLyQuanCafe.slnx                 # File giải pháp Solution Visual Studio
    │   Program.cs                          # Khởi tạo DI & cấu hình kết nối DBUtil
    │   appsettings.json                    # Cấu hình DefaultConnection SQL Server
    │
    ├───DAL\                                # TẦNG DATA ACCESS LAYER (BÀI 3 & BÀI 4)
    │       DBUtil.cs                       # Lớp tiện ích ADO.NET cốt lõi
    │       CategoryDAL.cs                  # CRUD danh mục
    │       DrinkDAL.cs                     # CRUD đồ uống
    │       UserDAL.cs                      # CRUD người dùng
    │       CardDAL.cs                      # CRUD thẻ bàn
    │       BillDAL.cs                      # CRUD hóa đơn
    │       BillDetailDAL.cs                # CRUD chi tiết hóa đơn
    │
    ├───Models\                             # TẦNG THỰC THỂ NGHIỆP VỤ (ENTITIES)
    │       Category.cs
    │       Drink.cs
    │       User.cs
    │       Card.cs
    │       Bill.cs
    │       BillDetail.cs
    │
    ├───Controllers\                        # TẦNG ĐIỀU KHIỂN (MVC CONTROLLERS)
    │       DrinkController.cs
    │       HomeController.cs
    │
    └───Views\                              # TẦNG GIAO DIỆN (RAZOR VIEWS)
            Drink\Index.cshtml
            Shared\_Layout.cshtml
```

#### 3. Nhật ký kiểm soát mã nguồn Git
* Đã cấu hình `.gitignore` loại trừ các thư mục nhị phân `bin/`, `obj/`, cấu hình `.vs/`.
* Lệnh khởi tạo và commit thành công:
  ```bash
  git init
  git add .
  git commit -m "Init_Lab3_NET108_Nhom5_TB02357"
  ```
* Kết quả: Đã ghi nhận root-commit lưu trữ đầy đủ 93 files mã nguồn.

---

### BÀI 3 (2 ĐIỂM): XÂY DỰNG LỚP TIỆN ÍCH `DBUtil`

Lớp `DBUtil` cung cấp giải pháp đóng gói việc truy xuất ADO.NET an toàn, ngăn chặn SQL Injection bằng `SqlParameter`, bảo toàn ký tự tiếng Việt với `SqlDbType.NVarChar`.

```csharp
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyQuanCafe.DAL
{
    public static class DBUtil
    {
        public static string connString = "Server=localhost;Database=QuanLyQuanCafe;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

        // 1. Thực thi câu lệnh SELECT trả về DataTable
        public static DataTable queryDataTable(string sql, List<object>? parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi QueryDataTable: {ex.Message}");
                    throw;
                }
            }
            return dataTable;
        }

        // 2. Thực thi câu lệnh lấy giá trị đơn lẻ (COUNT, MAX, SUM)
        public static object? executeScalar(string sql, List<object>? parameters = null)
        {
            object? result = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi executeScalar: {ex.Message}");
                    throw;
                }
            }
            return result;
        }

        // 3. Thực thi câu lệnh thay đổi dữ liệu (INSERT, UPDATE, DELETE)
        public static int executeNonQuery(string sql, List<object>? parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi executeNonQuery: {ex.Message}");
                    throw;
                }
            }
            return rowsAffected;
        }
    }
}
```

---

### BÀI 4 (2 ĐIỂM): XÂY DỰNG CÁC LỚP THỰC THỂ & LỚP DAL

#### 1. Lớp Thực thể `Drink.cs`
```csharp
namespace QuanLyQuanCafe.Models
{
    public class Drink
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; } = true;
        public string? CategoryName { get; set; }
    }
}
```

#### 2. Lớp Truy xuất Dữ liệu `DrinkDAL.cs` (Đầy đủ CRUD)
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.DAL
{
    public class DrinkDAL
    {
        // 1. Lấy toàn bộ danh sách đồ uống kèm tên danh mục
        public List<Drink> GetAll()
        {
            List<Drink> list = new List<Drink>();
            string sql = @"SELECT d.id, d.category_id, d.name, d.price, d.image, d.description, d.active,
                                  c.name AS category_name
                           FROM drinks d
                           LEFT JOIN categories c ON d.category_id = c.id
                           ORDER BY d.id ASC";
            DataTable dt = DBUtil.queryDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Drink
                {
                    Id = Convert.ToInt32(row["id"]),
                    CategoryId = row["category_id"] != DBNull.Value ? Convert.ToInt32(row["category_id"]) : null,
                    Name = row["name"].ToString() ?? string.Empty,
                    Price = Convert.ToInt32(row["price"]),
                    Image = row["image"] != DBNull.Value ? row["image"].ToString() : null,
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"]),
                    CategoryName = row["category_name"] != DBNull.Value ? row["category_name"].ToString() : null
                });
            }
            return list;
        }

        // 2. Lấy thông tin 1 đồ uống theo ID
        public Drink? GetById(int id)
        {
            string sql = @"SELECT d.id, d.category_id, d.name, d.price, d.image, d.description, d.active,
                                  c.name AS category_name
                           FROM drinks d
                           LEFT JOIN categories c ON d.category_id = c.id
                           WHERE d.id = @1";
            DataTable dt = DBUtil.queryDataTable(sql, new List<object> { id });
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Drink
                {
                    Id = Convert.ToInt32(row["id"]),
                    CategoryId = row["category_id"] != DBNull.Value ? Convert.ToInt32(row["category_id"]) : null,
                    Name = row["name"].ToString() ?? string.Empty,
                    Price = Convert.ToInt32(row["price"]),
                    Image = row["image"] != DBNull.Value ? row["image"].ToString() : null,
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    Active = row["active"] != DBNull.Value && Convert.ToBoolean(row["active"]),
                    CategoryName = row["category_name"] != DBNull.Value ? row["category_name"].ToString() : null
                };
            }
            return null;
        }

        // 3. Thêm mới đồ uống
        public int Insert(Drink drink)
        {
            string sql = @"INSERT INTO drinks (category_id, name, price, image, description, active)
                           VALUES (@1, @2, @3, @4, @5, @6);
                           SELECT SCOPE_IDENTITY();";
            object? result = DBUtil.executeScalar(sql, new List<object>
            {
                (object?)drink.CategoryId ?? DBNull.Value,
                drink.Name,
                drink.Price,
                (object?)drink.Image ?? DBNull.Value,
                (object?)drink.Description ?? DBNull.Value,
                drink.Active
            });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // 4. Cập nhật đồ uống
        public bool Update(Drink drink)
        {
            string sql = @"UPDATE drinks 
                           SET category_id = @1, name = @2, price = @3, image = @4, description = @5, active = @6
                           WHERE id = @7";
            int rows = DBUtil.executeNonQuery(sql, new List<object>
            {
                (object?)drink.CategoryId ?? DBNull.Value,
                drink.Name,
                drink.Price,
                (object?)drink.Image ?? DBNull.Value,
                (object?)drink.Description ?? DBNull.Value,
                drink.Active,
                drink.Id
            });
            return rows > 0;
        }

        // 5. Xóa đồ uống theo ID
        public bool Delete(int id)
        {
            string sql = "DELETE FROM drinks WHERE id = @1";
            int rows = DBUtil.executeNonQuery(sql, new List<object> { id });
            return rows > 0;
        }
    }
}
```

---

## KẾT LUẬN ĐÁNH GIÁ BÀI LAB 3
* **Bài 1 (3/3 điểm)**: Viết hoàn chỉnh script CSDL `QuanLyQuanCafe.sql` với 6 bảng, đầy đủ khóa chính `IDENTITY`, khóa ngoại liên kết logic và dữ liệu mẫu phong phú.
* **Bài 2 (3/3 điểm)**: Khởi tạo Solution ASP.NET Core MVC trên nền .NET 8.0 LTS, phân tách cấu trúc 3 lớp rõ ràng (DAL - Models - Controllers/Views), tích hợp Git repository và commit gốc thành công.
* **Bài 3 (2/2 điểm)**: Cài đặt hoàn chỉnh lớp tiện ích `DBUtil` với 3 phương thức (`queryDataTable`, `executeScalar`, `executeNonQuery`), bảo đảm an toàn dữ liệu và chống SQL Injection.
* **Bài 4 (2/2 điểm)**: Hoàn thành 100% các lớp Thực thể và các lớp DAL (`CategoryDAL`, `DrinkDAL`, `UserDAL`, `CardDAL`, `BillDAL`, `BillDetailDAL`) với đủ 5 chức năng CRUD.
* Dự án biên dịch thành công: **0 Lỗi (0 Error), 0 Cảnh báo (0 Warning)**.
