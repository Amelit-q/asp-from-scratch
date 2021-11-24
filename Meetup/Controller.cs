using Microsoft.AspNetCore.Mvc;

namespace AspFromScratch.WebApi.Meetup;

[ApiController]
[Route("/meetups")]
public class MeetupController : ControllerBase
{
    private static readonly ICollection<MeetupEntity> Meetups = new List<MeetupEntity>();


    /// <summary>Creates new meetup</summary>
    /// <response code="200">Meetup was created</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    public IActionResult CreateMeetup([FromBody] CreateMeetupDto createDto)
    {
        var newMeetup = new MeetupEntity
        {
            Id = Guid.NewGuid(),
            Topic = createDto.Topic,
            Place = createDto.Place,
            Duration = createDto.Duration
        };
        Meetups.Add(newMeetup);

        var readDto = new ReadMeetupDto
        {
            Id = newMeetup.Id,
            Topic = newMeetup.Topic,
            Place = newMeetup.Place,
            Duration = newMeetup.Duration
        };
        return Ok(readDto);
    }

    /// <summary>Update information about meetup</summary>
    /// <param name="id" example="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">Update meetup by ID</param>
    /// <response code="200">Update information about meetup</response>
    /// <response code="404">Meetup was not found</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateMeetup([FromRoute] Guid id, [FromBody] UpdateMeetupDto updateDto)
    {
        var prevMeetup = Meetups.SingleOrDefault(meetup => meetup.Id == id);

        if (prevMeetup is null)
        {
            return NotFound();
        }

        prevMeetup.Topic = updateDto.Topic;
        prevMeetup.Duration = updateDto.Duration;
        prevMeetup.Place = updateDto.Place;

        return NoContent();
    }

    /// <summary>Get all meetups</summary>
    /// <response code="200">List of the meetups</response>
    /// <response code="404">Meetups was not found</response>
    [HttpGet]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMeetups()
    {
        var readDtos = Meetups.Select(meetups => new ReadMeetupDto
        {
            Id = meetups.Id,
            Topic = meetups.Topic,
            Duration = meetups.Duration,
            Place = meetups.Place
        });

        return Ok(readDtos);
    }

    /// <summary>Delete meetup by id</summary>
    /// <param name="id" example="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">Meetup Id which suppose to be deleted</param>
    /// <response code="200">Meetup was successfully deleted</response>
    /// <response code="400">Meetup was not found</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteMeetup([FromRoute] Guid id)
    {
        var meetupToDelete = Meetups.SingleOrDefault(meetup => meetup.Id == id);

        if (meetupToDelete is null)
        {
            return NotFound();
        }

        Meetups.Remove(meetupToDelete);

        var readDto = new ReadMeetupDto
        {
            Id = meetupToDelete.Id,
            Duration = meetupToDelete.Duration,
            Place = meetupToDelete.Place,
            Topic = meetupToDelete.Topic
        };


        return Ok(readDto);
    }
}
