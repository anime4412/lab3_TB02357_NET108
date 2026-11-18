using System;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    /// <summary>
    /// CODE FIRST DBCONTEXT: Ánh xạ toàn bộ các thực thể C# xuống SQL Server
    /// Đề tài: Hệ thống Quản lý Thư viện Điện tử (Library Management System)
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowSlip> BorrowSlips { get; set; }
        public DbSet<BorrowDetail> BorrowDetails { get; set; }
        public DbSet<Fine> Fines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình Index Unique tránh trùng lặp dữ liệu
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeCode).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();

            modelBuilder.Entity<Reader>().HasIndex(r => r.ReaderCode).IsUnique();
            modelBuilder.Entity<Reader>().HasIndex(r => r.Email).IsUnique();

            modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
            modelBuilder.Entity<BorrowSlip>().HasIndex(bs => bs.SlipCode).IsUnique();

            // 2. Cấu hình quan hệ khóa ngoại và chống xung đột Multiple Cascade Paths
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Shelf)
                .WithMany(s => s.Books)
                .HasForeignKey(b => b.ShelfId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BorrowSlip>()
                .HasOne(bs => bs.Reader)
                .WithMany(r => r.BorrowSlips)
                .HasForeignKey(bs => bs.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowSlip>()
                .HasOne(bs => bs.Employee)
                .WithMany(e => e.BorrowSlips)
                .HasForeignKey(bs => bs.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BorrowDetail>()
                .HasOne(bd => bd.BorrowSlip)
                .WithMany(bs => bs.BorrowDetails)
                .HasForeignKey(bd => bd.BorrowSlipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowDetail>()
                .HasOne(bd => bd.Book)
                .WithMany(b => b.BorrowDetails)
                .HasForeignKey(bd => bd.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.BorrowSlip)
                .WithMany(bs => bs.Fines)
                .HasForeignKey(f => f.BorrowSlipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.Reader)
                .WithMany(r => r.Fines)
                .HasForeignKey(f => f.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Seed Data mẫu chuẩn Code First (HasData)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Công nghệ thông tin", Description = "Sách lập trình, cơ sở dữ liệu, mạng máy tính, AI", Active = true },
                new Category { Id = 2, Name = "Kinh tế & Quản trị", Description = "Quản trị kinh doanh, tài chính, khởi nghiệp", Active = true },
                new Category { Id = 3, Name = "Văn học & Nghệ thuật", Description = "Tiểu thuyết, truyện ngắn, tác phẩm kinh điển", Active = true },
                new Category { Id = 4, Name = "Kỹ năng sống & Phát triển bản thân", Description = "Tư duy, giao tiếp, tâm lý học ứng dụng", Active = true },
                new Category { Id = 5, Name = "Khoa học cơ bản", Description = "Toán học, vật lý, thiên văn học", Active = true }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Robert C. Martin (Uncle Bob)", Biography = "Chuyên gia kiến trúc phần mềm, tác giả Clean Code", Active = true },
                new Author { Id = 2, Name = "Andrew Hunt & David Thomas", Biography = "Tác giả cuốn The Pragmatic Programmer", Active = true },
                new Author { Id = 3, Name = "Dale Carnegie", Biography = "Tác giả Đắc Nhân Tâm", Active = true },
                new Author { Id = 4, Name = "Nguyễn Nhật Ánh", Biography = "Nhà văn nổi tiếng Việt Nam", Active = true },
                new Author { Id = 5, Name = "Yuval Noah Harari", Biography = "Tác giả Sapiens: Lược sử loài người", Active = true }
            );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "NXB Thông tin và Truyền thông", Address = "Hà Nội", Phone = "02435770000", Email = "nxbtt@gmail.com", Active = true },
                new Publisher { Id = 2, Name = "NXB Trẻ", Address = "TP. Hồ Chí Minh", Phone = "02839316289", Email = "nxbtre@hcm.fpt.vn", Active = true },
                new Publisher { Id = 3, Name = "NXB Kim Đồng", Address = "Hà Nội", Phone = "02439434730", Email = "info@nxbkimdong.com.vn", Active = true },
                new Publisher { Id = 4, Name = "NXB Khoa học và Kỹ thuật", Address = "Hà Nội", Phone = "02438253164", Email = "nxbkhkt@hn.vnn.vn", Active = true }
            );

            modelBuilder.Entity<Shelf>().HasData(
                new Shelf { Id = 1, Name = "Kệ A1 - CNTT", Location = "Tầng 2 - Dãy A bên trái", Active = true },
                new Shelf { Id = 2, Name = "Kệ A2 - Lập trình .NET", Location = "Tầng 2 - Dãy A bên phải", Active = true },
                new Shelf { Id = 3, Name = "Kệ B1 - Kinh tế", Location = "Tầng 1 - Dãy B trung tâm", Active = true },
                new Shelf { Id = 4, Name = "Kệ C1 - Văn học", Location = "Tầng 1 - Dãy C", Active = true }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, EmployeeCode = "NV001", FullName = "Nguyễn Quản Trị", Email = "admin@thuvien.edu.vn", Password = "Admin@123", Phone = "0911223344", Role = "Admin", Active = true },
                new Employee { Id = 2, EmployeeCode = "NV002", FullName = "Trần Thị Thủ Thư", Email = "thuthu01@thuvien.edu.vn", Password = "Thuthu@123", Phone = "0988776655", Role = "Librarian", Active = true }
            );

            var seedRegisterDate = new DateTime(2026, 1, 1);
            var seedExpiryDate = new DateTime(2027, 1, 1);

            modelBuilder.Entity<Reader>().HasData(
                new Reader { Id = 1, ReaderCode = "DG001", FullName = "Trần Minh Tuấn", Email = "tuan.tm@gmail.com", Password = "123456", Phone = "0933112233", Address = "Cầu Giấy, Hà Nội", IdentityNumber = "001202001122", RegisterDate = seedRegisterDate, ExpiryDate = seedExpiryDate, MaxBorrowLimit = 5, Active = true },
                new Reader { Id = 2, ReaderCode = "DG002", FullName = "Lê Hoàng Yến", Email = "yen.lh@gmail.com", Password = "123456", Phone = "0944223344", Address = "Bắc Từ Liêm, Hà Nội", IdentityNumber = "001202003344", RegisterDate = seedRegisterDate, ExpiryDate = seedExpiryDate, MaxBorrowLimit = 5, Active = true }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, ISBN = "9780132350884", Title = "Clean Code: A Handbook of Agile Software Craftsmanship", CategoryId = 1, AuthorId = 1, PublisherId = 1, ShelfId = 2, PublishYear = 2021, PageCount = 464, Price = 250000, Quantity = 10, AvailableQuantity = 8, CoverImage = "cleancode.jpg", Description = "Cuốn sách kinh điển về kỹ nghệ viết mã sạch, tối ưu và dễ bảo trì.", Active = true },
                new Book { Id = 2, ISBN = "9780201616224", Title = "The Pragmatic Programmer: Your Journey To Mastery", CategoryId = 1, AuthorId = 2, PublisherId = 1, ShelfId = 2, PublishYear = 2022, PageCount = 352, Price = 280000, Quantity = 8, AvailableQuantity = 7, CoverImage = "pragmatic.jpg", Description = "Hành trình từ lập trình viên nghiệp dư trở thành bậc thầy phát triển phần mềm.", Active = true },
                new Book { Id = 3, ISBN = "9786045882885", Title = "Đắc Nhân Tâm (How to Win Friends and Influence People)", CategoryId = 4, AuthorId = 3, PublisherId = 2, ShelfId = 3, PublishYear = 2023, PageCount = 320, Price = 95000, Quantity = 15, AvailableQuantity = 14, CoverImage = "dacnhantam.jpg", Description = "Nghệ thuật thu phục lòng người và xây dựng mối quan hệ bền vững.", Active = true },
                new Book { Id = 4, ISBN = "9786042171120", Title = "Mắt Biếc", CategoryId = 3, AuthorId = 4, PublisherId = 3, ShelfId = 4, PublishYear = 2020, PageCount = 300, Price = 110000, Quantity = 12, AvailableQuantity = 11, CoverImage = "matbiec.jpg", Description = "Tác phẩm văn học lãng mạn đầy cảm xúc của nhà văn Nguyễn Nhật Ánh.", Active = true },
                new Book { Id = 5, ISBN = "9780062316097", Title = "Sapiens: Lược Sử Loài Người", CategoryId = 5, AuthorId = 5, PublisherId = 4, ShelfId = 3, PublishYear = 2022, PageCount = 512, Price = 185000, Quantity = 10, AvailableQuantity = 10, CoverImage = "sapiens.jpg", Description = "Khám phá toàn cảnh lịch sử tiến hóa và văn minh nhân loại.", Active = true }
            );

            modelBuilder.Entity<BorrowSlip>().HasData(
                new BorrowSlip { Id = 1, SlipCode = "PM0001", ReaderId = 1, EmployeeId = 2, BorrowDate = new DateTime(2026, 11, 1), DueDate = new DateTime(2026, 11, 15), Status = "Borrowing", DepositAmount = 100000, Notes = "Mượn 2 cuốn sách CNTT kỳ 1" },
                new BorrowSlip { Id = 2, SlipCode = "PM0002", ReaderId = 2, EmployeeId = 2, BorrowDate = new DateTime(2026, 11, 2), DueDate = new DateTime(2026, 11, 16), Status = "Borrowing", DepositAmount = 50000, Notes = "Mượn sách văn học" }
            );

            modelBuilder.Entity<BorrowDetail>().HasData(
                new BorrowDetail { Id = 1, BorrowSlipId = 1, BookId = 1, Quantity = 1, RentalPrice = 10000, IsReturned = false, ConditionStatus = "Nguyên vẹn", FineAmount = 0 },
                new BorrowDetail { Id = 2, BorrowSlipId = 1, BookId = 2, Quantity = 1, RentalPrice = 10000, IsReturned = false, ConditionStatus = "Nguyên vẹn", FineAmount = 0 },
                new BorrowDetail { Id = 3, BorrowSlipId = 2, BookId = 4, Quantity = 1, RentalPrice = 5000, IsReturned = false, ConditionStatus = "Nguyên vẹn", FineAmount = 0 }
            );
        }
    }
}
