using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class Reader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ReaderCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? IdentityNumber { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddYears(1);

        public int MaxBorrowLimit { get; set; } = 5;

        public bool Active { get; set; } = true;

        // Navigation properties
        public virtual ICollection<BorrowSlip> BorrowSlips { get; set; } = new List<BorrowSlip>();
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
    }
}
