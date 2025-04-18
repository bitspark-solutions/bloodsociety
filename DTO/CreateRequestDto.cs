namespace bloodsociety.DTO
{
    public class CreateRequestDto
    {
        public int RequesterId { get; set; }
        public string BloodType { get; set; }
        public string Location { get; set; }
        public string Visibility { get; set; } // Public/Private
    }
}
