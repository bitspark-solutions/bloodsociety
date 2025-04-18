namespace bloodsociety.DTO
{
    public class GiveReviewRequest
    {
        public int DonorId { get; set; }
        public int ReceiverId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
