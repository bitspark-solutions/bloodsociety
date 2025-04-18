namespace bloodsociety.DTO
{
    public class SearchDonorsRequest
    {
        public string BloodType { get; set; }
        public bool? ActiveStatus { get; set; }
        public DateTime? LastDonatedAfter { get; set; }
        public DateTime? LastDonatedBefore { get; set; }
        public string Location { get; set; }
    }
}
