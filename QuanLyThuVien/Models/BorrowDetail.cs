using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class BorrowDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BorrowSlipId { get; set; }

        [Required]
        public int BookId { get; set; }

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal RentalPrice { get; set; } = 0;

        public bool IsReturned { get; set; } = false;
        public DateTime? ReturnDate { get; set; }

        [StringLength(100)]
        public string ConditionStatus { get; set; } = "Nguyên vẹn";

        [Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; } = 0;

        // Navigation properties Code First
        [ForeignKey("BorrowSlipId")]
        public virtual BorrowSlip? BorrowSlip { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [NotMapped]
        public string? BookTitle => Book?.Title;

        [NotMapped]
        public string? BookISBN => Book?.ISBN;
    }
}
