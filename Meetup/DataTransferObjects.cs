using System.ComponentModel.DataAnnotations;

namespace AspFromScratch.WebApi.Meetup;

public class ReadMeetupDto
{
    public Guid Id { get; set; }
    public string Topic { get; set; }
    public string Place { get; set; }
    public int Duration { get; set; }
}

public class CreateMeetupDto
{
    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^[\w\s\.-–—]*$")]
    public string Topic { get; set; }

    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^[\w\s\.\d]*")]
    public string Place { get; set; }

    [Required] [Range(30, 300)]
    public int Duration { get; set; }
}

public class UpdateMeetupDto
{
    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^[\w\s\.-–—]*$")]
    public string Topic { get; set; }

    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^[\w\s\.\d]*")]
    public string Place { get; set; }

    [Required] [Range(30, 300)]
    public int Duration { get; set; }
}
