namespace bloodsociety.Models
{
    public class CacheEntry
    {
        public string CacheKey { get; set; }
        public string CacheValue { get; set; }
        public DateTime Expiration { get; set; }
    }
}
