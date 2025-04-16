namespace bloodsociety.Models
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; } // FK to User
        public int ReceiverId { get; set; } // FK to User
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
