namespace AspFromScratch.WebApi.Meetup;

internal class Meetup
{
    /// <summary>Meetup.id</summary>
    /// <example>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</example>
    public Guid Id { get; set; }

    /// <summary>Topic discussed on meetup</summary>
    /// <example>Microsoft naming issues</example>
    public string Topic { get; set; }

    /// <summary>Meetup location</summary>
    /// <example>Oslo</example>
    public string Place { get; set; }

    /// <summary>Meetup duration in minutes</summary>
    /// <example>180</example>
    public int Duration { get; set; }
}
