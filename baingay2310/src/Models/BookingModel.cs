using System;

namespace hoccsharp.Models
{
    public class BookingModel
    {
        public string BookingId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public int Days { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalAmount => Days * PricePerNight;
    }
}