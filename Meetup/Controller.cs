using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspFromScratch.WebApi.Meetup;

[ApiController]
[Route("/meetups")]
public class MeetupController : ControllerBase
{
    private readonly DatabaseContext _context = new DatabaseContext();

    /// <summary>Creates new meetup</summary>
    /// <response code="200">Meetup was created</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateMeetup([FromBody] CreateMeetupDto createDto)
    {
        var newMeetup = new MeetupEntity
        {
            Id = Guid.NewGuid(),
            Topic = createDto.Topic,
            Place = createDto.Place,
            Duration = createDto.Duration
        };

        _context.Meetups.Add(newMeetup);
        await _context.SaveChangesAsync();

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
    public async Task<IActionResult> UpdateMeetup([FromRoute] Guid id, [FromBody] UpdateMeetupDto updateDto)
    {
        // var prevMeetup = Meetups.SingleOrDefault(meetup => meetup.Id == id);

        var prevMeetup = await _context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);

        if (prevMeetup is null)
        {
            return NotFound();
        }

        prevMeetup.Topic = updateDto.Topic;
        prevMeetup.Duration = updateDto.Duration;
        prevMeetup.Place = updateDto.Place;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>Get all meetups</summary>
    /// <response code="200">List of the meetups</response>
    /// <response code="404">Meetups was not found</response>
    [HttpGet]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMeetups()
    {
        var meetups = await _context.Meetups.ToListAsync();

        var readDtos = meetups.Select(meetup => new ReadMeetupDto
        {
            Id = meetup.Id,
            Topic = meetup.Topic,
            Duration = meetup.Duration,
            Place = meetup.Place
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
    public async Task<IActionResult> DeleteMeetup([FromRoute] Guid id)
    {
        // var meetupToDelete = Meetups.SingleOrDefault(meetup => meetup.Id == id);

        var meetupToDelete = await _context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);

        if (meetupToDelete is null)
        {
            return NotFound();
        }

        _context.Meetups.Remove(meetupToDelete);
        await _context.SaveChangesAsync();

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
