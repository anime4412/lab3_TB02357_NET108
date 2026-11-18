# BÁO CÁO THỰC HÀNH LAB 3: THIẾT KẾ MÔ HÌNH 3 LỚP & TRUY XUẤT CSDL VỚI ENTITY FRAMEWORK CORE (CODE FIRST)
**MÔN HỌC: DỰ ÁN .NET 1 (NET108 / SOF1022)**  
**LỚP: DỰ ÁN 1 — NHÓM THỰC HIỆN: NHÓM 5**  
**MÃ SINH VIÊN: TB02357**  
**ĐỀ TÀI: HỆ THỐNG QUẢN LÝ THƯ VIỆN ĐIỆN TỬ (LIBRARY MANAGEMENT SYSTEM)**  

---

## MỤC TIÊU BÀI THỰC HÀNH LAB 3 (CODE FIRST)
1. **Xây dựng Cơ sở Dữ liệu theo hướng Code First**: Định nghĩa toàn bộ các thực thể C# kèm Data Annotations (`[Key]`, `[Required]`, `[StringLength]`, `[Column]`, `[ForeignKey]`) và cấu hình Fluent API + Seed Data (`HasData`) trong `LibraryDbContext`.
2. **Quản lý phiên bản CSDL bằng EF Core Migrations**: Sử dụng lệnh `Add-Migration InitialCreate` (`dotnet ef migrations add InitialCreate`) và `Update-Database` để tự động sinh bảng và dữ liệu mẫu xuống SQL Server.
3. **Thiết kế kiến trúc 3 lớp + Dependency Injection**:
   - **Tầng Data (`Data/LibraryDbContext.cs`)**: Kết nối SQL Server qua Entity Framework Core 8.0.
   - **Tầng Models (`Models/`)**: Các Entity ánh xạ chuẩn xác 10 bảng thư viện: `Book`, `Category`, `Author`, `Publisher`, `Shelf`, `Reader`, `Employee`, `BorrowSlip`, `BorrowDetail`, `Fine`.
   - **Tầng DAL (`DAL/`)**: Đóng gói thao tác LINQ & EF Core CRUD (`BookDAL`, `CategoryDAL`, `ReaderDAL`, `BorrowSlipDAL`) và tiêm phụ thuộc (DI) vào Controller.
   - **Tầng Presentation (`Controllers/`, `Views/`)**: Giao diện điều hướng `BookController` và Razor View Bootstrap 5.
4. **Quản lý mã nguồn bằng Git**: Cấu hình `.gitignore`, commit mã nguồn Code First & Migrations lên GitHub.

---

## NỘI DUNG THỰC HIỆN LAB 3 (EF CORE CODE FIRST)

### BÀI 1 (3 ĐIỂM): THIẾT KẾ ENTITY MODELS & TẠO CSDL BẰNG CODE FIRST MIGRATIONS

#### 1. Các lớp Thực thể (Entities) với Data Annotations
Hệ thống Thư viện bao gồm 10 thực thể ánh xạ thành 10 bảng trong SQL Server:
* `Employees`, `Readers`, `Categories`, `Authors`, `Publishers`, `Shelves`, `Books`, `BorrowSlips`, `BorrowDetails`, `Fines`.

Ví dụ lớp Thực thể `Book.cs`:
```csharp
public class Book
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Mã ISBN không được để trống")]
    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên sách không được để trống")]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;

    public int? CategoryId { get; set; }
    public int? AuthorId { get; set; }
    public int? PublisherId { get; set; }
    public int? ShelfId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    public int Quantity { get; set; } = 1;
    public int AvailableQuantity { get; set; } = 1;

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
    [ForeignKey("AuthorId")]
    public virtual Author? Author { get; set; }
}
```

#### 2. Sinh CSDL bằng Migrations
* Lệnh tạo Migration: `dotnet ef migrations add InitialCreate` (sinh ra thư mục `Migrations/` chứa file `InitialCreate.cs` và `LibraryDbContextModelSnapshot.cs`).
* Lệnh cập nhật xuống SQL Server: `dotnet ef database update`.
* Script SQL được xuất tự động từ Code First Migration: `QuanLyThuVien_Migration.sql`.

---

### BÀI 2 (3 ĐIỂM): KHỞI TẠO DỰ ÁN 3 LỚP VỚI DI & QUẢN LÝ BẰNG GIT

#### Cấu trúc Solution ASP.NET Core 8.0 MVC (`QuanLyThuVien`)
```text
D:\test\lab3_TB02357_NET108\
│   .gitignore                              # Cấu hình loại trừ file build
│   QuanLyThuVien_Migration.sql             # Script SQL xuất tự động từ EF Core Code First
│   BAOCAO_LAB3_TB02357_NET108.docx         # Báo cáo Word chính thức
│   BAOCAO_LAB3_TB02357_NET108.md           # Báo cáo Markdown
│   lab3_TB02357_NET108.zip                 # File ZIP nén sẵn nộp LMS
│
└───QuanLyThuVien\
    │   QuanLyThuVien.csproj                # Cài đặt EF Core SqlServer, Tools, Design 8.0.11
    │   Program.cs                          # Đăng ký AddDbContext & DI AddScoped cho các lớp DAL
    │   appsettings.json                    # Chuỗi kết nối DefaultConnection
    │
    ├───Data\
    │       LibraryDbContext.cs             # DbContext, Fluent API, Indexes & Seed Data HasData
    │
    ├───Migrations\                         # Code First Migrations tự sinh bởi dotnet-ef
    │       ..._InitialCreate.cs
    │       LibraryDbContextModelSnapshot.cs
    │
    ├───DAL\                                # TẦNG TRUY XUẤT DỮ LIỆU (LINQ + EF CORE)
    │       BookDAL.cs
    │       CategoryDAL.cs
    │       ReaderDAL.cs
    │       BorrowSlipDAL.cs
    │
    ├───Models\                             # CÁC ENTITY CODE FIRST
    │       Book.cs, Category.cs, Author.cs, Publisher.cs, Shelf.cs,
    │       Reader.cs, Employee.cs, BorrowSlip.cs, BorrowDetail.cs, Fine.cs
    │
    └───Controllers\ & Views\
            BookController.cs & Views/Book/Index.cshtml
```

---

### BÀI 3 (2 ĐIỂM): XÂY DỰNG `LibraryDbContext` (CODE FIRST) & SEED DATA

Thay vì dùng `DBUtil` thuần, dự án sử dụng `LibraryDbContext` kế thừa `DbContext` để quản lý kết nối, ràng buộc duy nhất (`HasIndex(...).IsUnique()`), chống xung đột xóa dây chuyền (`DeleteBehavior.Restrict` / `SetNull`), và khởi tạo dữ liệu mẫu thông qua `HasData(...)`:

```csharp
// Cấu hình trong Program.cs
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BookDAL>();
builder.Services.AddScoped<CategoryDAL>();
builder.Services.AddScoped<ReaderDAL>();
builder.Services.AddScoped<BorrowSlipDAL>();
```

---

### BÀI 4 (2 ĐIỂM): CÁC LỚP DAL SỬ DỤNG EF CORE CODE FIRST (CRUD ĐẦY ĐỦ)

Ví dụ `BookDAL.cs`:
```csharp
public class BookDAL
{
    private readonly LibraryDbContext _context;

    public BookDAL(LibraryDbContext context)
    {
        _context = context;
    }

    public List<Book> GetAll() => _context.Books
        .Include(b => b.Category).Include(b => b.Author)
        .Include(b => b.Publisher).Include(b => b.Shelf)
        .OrderByDescending(b => b.Id).AsNoTracking().ToList();

    public Book? GetById(int id) => _context.Books
        .Include(b => b.Category).Include(b => b.Author)
        .FirstOrDefault(b => b.Id == id);

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
        existing.Title = book.Title;
        existing.Price = book.Price;
        existing.Quantity = book.Quantity;
        // ... cập nhật các trường khác
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
```

---

## TỔNG KẾT
* Toàn bộ dự án Lab 3 đã chuyển đổi hoàn chỉnh sang **Entity Framework Core 8.0 Code First** kèm **Migrations** và **Seed Data**.
* Biên dịch dự án thành công: **0 Warning(s), 0 Error(s)**.
