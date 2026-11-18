using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class Shelf
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Location { get; set; }

        public bool Active { get; set; } = true;

        // Navigation property
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
