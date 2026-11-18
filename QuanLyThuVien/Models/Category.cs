using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool Active { get; set; } = true;

        // Navigation property
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
