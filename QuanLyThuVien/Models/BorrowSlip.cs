using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class BorrowSlip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string SlipCode { get; set; } = string.Empty;

        [Required]
        public int ReaderId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);
        public DateTime? ActualReturnDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Borrowing";

        [Column(TypeName = "decimal(18,2)")]
        public decimal DepositAmount { get; set; } = 0;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties Code First
        [ForeignKey("ReaderId")]
        public virtual Reader? Reader { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

        [NotMapped]
        public string? ReaderName => Reader?.FullName;

        [NotMapped]
        public string? EmployeeName => Employee?.FullName;
    }
}
