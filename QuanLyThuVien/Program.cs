using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.DAL;
using QuanLyThuVien.Data;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Entity Framework Core Code First với SQL Server
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Dependency Injection cho tầng DAL (Scoped theo vòng đời DbContext)
builder.Services.AddScoped<BookDAL>();
builder.Services.AddScoped<CategoryDAL>();
builder.Services.AddScoped<ReaderDAL>();
builder.Services.AddScoped<BorrowSlipDAL>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();
