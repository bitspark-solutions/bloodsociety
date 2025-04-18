namespace bloodsociety.DTO
{
    public class UpdateRequestDto
    {
        public string BloodType { get; set; }
        public string Location { get; set; }
        public string Visibility { get; set; }
        public string Status { get; set; } // Open, Fulfilled, Cancelled
    }
}
