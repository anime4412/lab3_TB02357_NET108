using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class Fine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BorrowSlipId { get; set; }

        [Required]
        public int ReaderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        [StringLength(50)]
        public string Status { get; set; } = "Unpaid";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("BorrowSlipId")]
        public virtual BorrowSlip? BorrowSlip { get; set; }

        [ForeignKey("ReaderId")]
        public virtual Reader? Reader { get; set; }
    }
}
