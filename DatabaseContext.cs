using AspFromScratch.WebApi.Meetup;
using AspFromScratch.WebApi.User;
using Microsoft.EntityFrameworkCore;

namespace AspFromScratch.WebApi;

public class DatabaseContext : DbContext
{
    public DbSet<MeetupEntity> Meetups { get; set; }

    public DbSet<UserEntity> Users { get; set; }


    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}
