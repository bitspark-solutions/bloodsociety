namespace bloodsociety.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } // Donor, Receiver, Doctor, Admin, Staff
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // Active, Inactive
        public bool PhoneVerified { get; set; }
        public bool EmailVerified { get; set; }
    }
}
