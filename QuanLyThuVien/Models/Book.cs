namespace QuanLyThuVien.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int? ShelfId { get; set; }
        public int PublishYear { get; set; } = 2024;
        public int PageCount { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 1;
        public int AvailableQuantity { get; set; } = 1;
        public string? CoverImage { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; } = true;

        // Navigation properties hỗ trợ hiển thị
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public string? ShelfName { get; set; }
    }
}
