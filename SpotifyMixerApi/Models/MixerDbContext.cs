using Microsoft.EntityFrameworkCore;

namespace SpotifyMixerApi.Models
{
    public class MixerDbContext : DbContext
    {
        public MixerDbContext(DbContextOptions<MixerDbContext> options) : base(options) { }

        public DbSet<Mixer> Mixers { get; set; } = null!;
    }
} 