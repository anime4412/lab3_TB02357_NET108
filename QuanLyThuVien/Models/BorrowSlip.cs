using System;

namespace QuanLyThuVien.Models
{
    public class BorrowSlip
    {
        public int Id { get; set; }
        public string SlipCode { get; set; } = string.Empty;
        public int ReaderId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);
        public DateTime? ActualReturnDate { get; set; }
        public string Status { get; set; } = "Borrowing"; // Borrowing, Returned, Overdue, Cancelled
        public decimal DepositAmount { get; set; } = 0;
        public string? Notes { get; set; }

        // Navigation properties
        public string? ReaderName { get; set; }
        public string? EmployeeName { get; set; }
    }
}
