namespace QuanLyThuVien.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public bool Active { get; set; } = true;
    }
}
