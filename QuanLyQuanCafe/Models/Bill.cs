using System;

namespace QuanLyQuanCafe.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CardId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Total { get; set; }
        public string Status { get; set; } = "PAID";

        // Navigation properties hỗ trợ hiển thị
        public string? UserName { get; set; }
        public int? CardCode { get; set; }
    }
}
