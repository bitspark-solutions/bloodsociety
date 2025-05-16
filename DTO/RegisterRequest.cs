namespace bloodsociety.DTO
{
    public class RegisterRequest
    {
        public int BloodGroupId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } // Donor, Receiver, Doctor
    }
}
