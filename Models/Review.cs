namespace bloodsociety.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int DonorId { get; set; } // FK to User
        public int ReceiverId { get; set; } // FK to User
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Donor { get; set; }
        public User Receiver { get; set; }
    }
}
