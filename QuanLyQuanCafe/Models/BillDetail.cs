namespace QuanLyQuanCafe.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; } = 1;
        public int Price { get; set; }

        // Navigation property hỗ trợ hiển thị
        public string? DrinkName { get; set; }
    }
}
