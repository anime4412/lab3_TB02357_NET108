using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReaderCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxBorrowLimit = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shelves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowSlips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReaderId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowSlips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowSlips_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BorrowSlips_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    PublisherId = table.Column<int>(type: "int", nullable: true),
                    ShelfId = table.Column<int>(type: "int", nullable: true),
                    PublishYear = table.Column<int>(type: "int", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Books_Shelves_ShelfId",
                        column: x => x.ShelfId,
                        principalTable: "Shelves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Fines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowSlipId = table.Column<int>(type: "int", nullable: false),
                    ReaderId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fines_BorrowSlips_BorrowSlipId",
                        column: x => x.BorrowSlipId,
                        principalTable: "BorrowSlips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fines_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorrowDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowSlipId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RentalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConditionStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowDetails_BorrowSlips_BorrowSlipId",
                        column: x => x.BorrowSlipId,
                        principalTable: "BorrowSlips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Active", "Biography", "Name" },
                values: new object[,]
                {
                    { 1, true, "Chuyên gia kiến trúc phần mềm, tác giả Clean Code", "Robert C. Martin (Uncle Bob)" },
                    { 2, true, "Tác giả cuốn The Pragmatic Programmer", "Andrew Hunt & David Thomas" },
                    { 3, true, "Tác giả Đắc Nhân Tâm", "Dale Carnegie" },
                    { 4, true, "Nhà văn nổi tiếng Việt Nam", "Nguyễn Nhật Ánh" },
                    { 5, true, "Tác giả Sapiens: Lược sử loài người", "Yuval Noah Harari" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 1, true, "Sách lập trình, cơ sở dữ liệu, mạng máy tính, AI", "Công nghệ thông tin" },
                    { 2, true, "Quản trị kinh doanh, tài chính, khởi nghiệp", "Kinh tế & Quản trị" },
                    { 3, true, "Tiểu thuyết, truyện ngắn, tác phẩm kinh điển", "Văn học & Nghệ thuật" },
                    { 4, true, "Tư duy, giao tiếp, tâm lý học ứng dụng", "Kỹ năng sống & Phát triển bản thân" },
                    { 5, true, "Toán học, vật lý, thiên văn học", "Khoa học cơ bản" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Active", "Email", "EmployeeCode", "FullName", "Password", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, true, "admin@thuvien.edu.vn", "NV001", "Nguyễn Quản Trị", "Admin@123", "0911223344", "Admin" },
                    { 2, true, "thuthu01@thuvien.edu.vn", "NV002", "Trần Thị Thủ Thư", "Thuthu@123", "0988776655", "Librarian" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Active", "Address", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, true, "Hà Nội", "nxbtt@gmail.com", "NXB Thông tin và Truyền thông", "02435770000" },
                    { 2, true, "TP. Hồ Chí Minh", "nxbtre@hcm.fpt.vn", "NXB Trẻ", "02839316289" },
                    { 3, true, "Hà Nội", "info@nxbkimdong.com.vn", "NXB Kim Đồng", "02439434730" },
                    { 4, true, "Hà Nội", "nxbkhkt@hn.vnn.vn", "NXB Khoa học và Kỹ thuật", "02438253164" }
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "Id", "Active", "Address", "Email", "ExpiryDate", "FullName", "IdentityNumber", "MaxBorrowLimit", "Password", "Phone", "ReaderCode", "RegisterDate" },
                values: new object[,]
                {
                    { 1, true, "Cầu Giấy, Hà Nội", "tuan.tm@gmail.com", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trần Minh Tuấn", "001202001122", 5, "123456", "0933112233", "DG001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, true, "Bắc Từ Liêm, Hà Nội", "yen.lh@gmail.com", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Hoàng Yến", "001202003344", 5, "123456", "0944223344", "DG002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Shelves",
                columns: new[] { "Id", "Active", "Location", "Name" },
                values: new object[,]
                {
                    { 1, true, "Tầng 2 - Dãy A bên trái", "Kệ A1 - CNTT" },
                    { 2, true, "Tầng 2 - Dãy A bên phải", "Kệ A2 - Lập trình .NET" },
                    { 3, true, "Tầng 1 - Dãy B trung tâm", "Kệ B1 - Kinh tế" },
                    { 4, true, "Tầng 1 - Dãy C", "Kệ C1 - Văn học" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Active", "AuthorId", "AvailableQuantity", "CategoryId", "CoverImage", "Description", "ISBN", "PageCount", "Price", "PublishYear", "PublisherId", "Quantity", "ShelfId", "Title" },
                values: new object[,]
                {
                    { 1, true, 1, 8, 1, "cleancode.jpg", "Cuốn sách kinh điển về kỹ nghệ viết mã sạch, tối ưu và dễ bảo trì.", "9780132350884", 464, 250000m, 2021, 1, 10, 2, "Clean Code: A Handbook of Agile Software Craftsmanship" },
                    { 2, true, 2, 7, 1, "pragmatic.jpg", "Hành trình từ lập trình viên nghiệp dư trở thành bậc thầy phát triển phần mềm.", "9780201616224", 352, 280000m, 2022, 1, 8, 2, "The Pragmatic Programmer: Your Journey To Mastery" },
                    { 3, true, 3, 14, 4, "dacnhantam.jpg", "Nghệ thuật thu phục lòng người và xây dựng mối quan hệ bền vững.", "9786045882885", 320, 95000m, 2023, 2, 15, 3, "Đắc Nhân Tâm (How to Win Friends and Influence People)" },
                    { 4, true, 4, 11, 3, "matbiec.jpg", "Tác phẩm văn học lãng mạn đầy cảm xúc của nhà văn Nguyễn Nhật Ánh.", "9786042171120", 300, 110000m, 2020, 3, 12, 4, "Mắt Biếc" },
                    { 5, true, 5, 10, 5, "sapiens.jpg", "Khám phá toàn cảnh lịch sử tiến hóa và văn minh nhân loại.", "9780062316097", 512, 185000m, 2022, 4, 10, 3, "Sapiens: Lược Sử Loài Người" }
                });

            migrationBuilder.InsertData(
                table: "BorrowSlips",
                columns: new[] { "Id", "ActualReturnDate", "BorrowDate", "DepositAmount", "DueDate", "EmployeeId", "Notes", "ReaderId", "SlipCode", "Status" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100000m, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Mượn 2 cuốn sách CNTT kỳ 1", 1, "PM0001", "Borrowing" },
                    { 2, null, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 50000m, new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Mượn sách văn học", 2, "PM0002", "Borrowing" }
                });

            migrationBuilder.InsertData(
                table: "BorrowDetails",
                columns: new[] { "Id", "BookId", "BorrowSlipId", "ConditionStatus", "FineAmount", "IsReturned", "Quantity", "RentalPrice", "ReturnDate" },
                values: new object[,]
                {
                    { 1, 1, 1, "Nguyên vẹn", 0m, false, 1, 10000m, null },
                    { 2, 2, 1, "Nguyên vẹn", 0m, false, 1, 10000m, null },
                    { 3, 4, 2, "Nguyên vẹn", 0m, false, 1, 5000m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ShelfId",
                table: "Books",
                column: "ShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowDetails_BookId",
                table: "BorrowDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowDetails_BorrowSlipId",
                table: "BorrowDetails",
                column: "BorrowSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowSlips_EmployeeId",
                table: "BorrowSlips",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowSlips_ReaderId",
                table: "BorrowSlips",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowSlips_SlipCode",
                table: "BorrowSlips",
                column: "SlipCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fines_BorrowSlipId",
                table: "Fines",
                column: "BorrowSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_Fines_ReaderId",
                table: "Fines",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_Email",
                table: "Readers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ReaderCode",
                table: "Readers",
                column: "ReaderCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowDetails");

            migrationBuilder.DropTable(
                name: "Fines");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "BorrowSlips");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Shelves");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Readers");
        }
    }
}
