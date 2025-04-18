namespace bloodsociety.DTO
{
    public class UploadDocRequest
    {
        public int DonorId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
