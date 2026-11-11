namespace QuanLyThuVien.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public bool Active { get; set; } = true;
    }
}
