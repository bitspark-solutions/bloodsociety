namespace bloodsociety.Models
{
    public class BloodRequest
    {
        public int RequestId { get; set; }
        public int RequesterId { get; set; } // FK to User
        public string BloodType { get; set; }
        public string Location { get; set; }
        public string Visibility { get; set; } // Public/Private
        public string Status { get; set; } // Open, Fulfilled, Cancelled
        public DateTime CreatedAt { get; set; }
    }
}
