using AspFromScratch.WebApi.Meetup;
using Microsoft.EntityFrameworkCore;

namespace AspFromScratch.WebApi;

public class DatabaseContext : DbContext
{
    public DbSet<MeetupEntity> Meetups { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}
