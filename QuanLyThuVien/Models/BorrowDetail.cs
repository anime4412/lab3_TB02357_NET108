using System;

namespace QuanLyThuVien.Models
{
    public class BorrowDetail
    {
        public int Id { get; set; }
        public int BorrowSlipId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal RentalPrice { get; set; } = 0;
        public bool IsReturned { get; set; } = false;
        public DateTime? ReturnDate { get; set; }
        public string ConditionStatus { get; set; } = "Nguyên vẹn";
        public decimal FineAmount { get; set; } = 0;

        // Navigation property
        public string? BookTitle { get; set; }
        public string? BookISBN { get; set; }
    }
}
