using System;

namespace bloodsociety.Models
{
    public class UserBlock
    {
        public int UserBlockId { get; set; }
        public int UserId { get; set; }
        public string Reason { get; set; }
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
        public int? BlockedByAdminId { get; set; } // Optional: who blocked
        public bool IsActive { get; set; } = true;
    }
}