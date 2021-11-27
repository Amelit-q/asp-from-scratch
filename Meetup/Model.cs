using System.ComponentModel.DataAnnotations.Schema;

namespace AspFromScratch.WebApi.Meetup;

[Table("meetups")]
public class MeetupEntity
{
    /// <summary>Meetup.id</summary>
    /// <example>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</example>
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Topic discussed on meetup</summary>
    /// <example>Microsoft naming issues</example>
    [Column("topic")]
    public string Topic { get; set; }

    /// <summary>Meetup location</summary>
    /// <example>Oslo</example>
    [Column("place")]
    public string Place { get; set; }

    /// <summary>Meetup duration in minutes</summary>
    /// <example>180</example>
    [Column("duration")]
    public int Duration { get; set; }
}
