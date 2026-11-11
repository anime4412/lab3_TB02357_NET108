namespace QuanLyQuanCafe.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public bool Status { get; set; } = true; // 1: Sẵn sàng, 0: Đang sử dụng
    }
}
