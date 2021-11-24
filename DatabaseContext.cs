using AspFromScratch.WebApi.Meetup;
using Microsoft.EntityFrameworkCore;

namespace AspFromScratch.WebApi;

internal class DatabaseContext : DbContext
{
    public DbSet<MeetupEntity> Meetups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseNpgsql("Server=localhost;Port=5432;Database=asp_courses;User Id=db_user;Password=db_user_password");
}
