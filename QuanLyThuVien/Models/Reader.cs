using System;

namespace QuanLyThuVien.Models
{
    public class Reader
    {
        public int Id { get; set; }
        public string ReaderCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddYears(1);
        public int MaxBorrowLimit { get; set; } = 5;
        public bool Active { get; set; } = true;
    }
}
