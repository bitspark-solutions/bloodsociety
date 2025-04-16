namespace bloodsociety.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; } // FK to User
        public string Type { get; set; } // Request, Review, Message
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public User User { get; set; }
    }
}
