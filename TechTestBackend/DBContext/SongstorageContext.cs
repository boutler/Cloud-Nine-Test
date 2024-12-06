using Microsoft.EntityFrameworkCore;

namespace TechTestBackend.DataBaseContext;

public class SongstorageContext : DbContext, IApiMarker
{
    public SongstorageContext(DbContextOptions<SongstorageContext> options)
        : base(options)
    {
    }

    public DbSet<Soptifysong> Songs { get; set; }
}