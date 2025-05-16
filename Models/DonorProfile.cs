namespace bloodsociety.Models
{
    public class DonorProfile
    {
        public int DonorId { get; set; } // PK, FK to User
        public bool ActiveStatus { get; set; }
        public DateTime? LastDonationDate { get; set; }
        public string HealthCertificate { get; set; }
        public string BloodType { get; set; } // A+, O-, etc.
    }
}
