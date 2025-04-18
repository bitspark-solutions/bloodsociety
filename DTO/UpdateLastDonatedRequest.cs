namespace bloodsociety.DTO
{
    public class UpdateLastDonatedRequest
    {
        public int DonorId { get; set; }
        public DateTime LastDonationDate { get; set; }
    }
}
