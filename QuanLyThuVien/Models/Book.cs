using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã ISBN không được để trống")]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int? ShelfId { get; set; }

        public int PublishYear { get; set; } = 2024;
        public int PageCount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        public int Quantity { get; set; } = 1;
        public int AvailableQuantity { get; set; } = 1;

        [StringLength(255)]
        public string? CoverImage { get; set; }

        public string? Description { get; set; }
        public bool Active { get; set; } = true;

        // Navigation properties Code First
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }

        [ForeignKey("PublisherId")]
        public virtual Publisher? Publisher { get; set; }

        [ForeignKey("ShelfId")]
        public virtual Shelf? Shelf { get; set; }

        public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();

        // Thuộc tính không ánh xạ CSDL hỗ trợ hiển thị View
        [NotMapped]
        public string? CategoryName => Category?.Name;

        [NotMapped]
        public string? AuthorName => Author?.Name;

        [NotMapped]
        public string? PublisherName => Publisher?.Name;

        [NotMapped]
        public string? ShelfName => Shelf?.Name;
    }
}
