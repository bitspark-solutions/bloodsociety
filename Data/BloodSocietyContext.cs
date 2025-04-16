using Microsoft.EntityFrameworkCore;
using bloodsociety.Models;

namespace bloodsociety.Data
{
    public class BloodSocietyContext : DbContext
    {
        public BloodSocietyContext(DbContextOptions<BloodSocietyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<DonorProfile> DonorProfiles { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        // Optionally: public DbSet<CacheEntry> CacheEntries { get; set; }
    }
}
