namespace QuanLyQuanCafe.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public bool Role { get; set; } // 0: Nhân viên, 1: Quản lý
        public bool Active { get; set; } = true;
    }
}
