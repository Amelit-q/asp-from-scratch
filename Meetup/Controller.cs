using Microsoft.AspNetCore.Mvc;

namespace AspFromScratch.WebApi.Meetup;

[ApiController]
[Route("/meetups")]
public class MeetupController : ControllerBase
{
    private static readonly ICollection<Meetup> Meetups = new List<Meetup>();


    [HttpPost]
    public IActionResult CreateMeetup([FromBody] CreateMeetupDto createDto)
    {
        var newMeetup = new Meetup
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

    [HttpPut("{id:guid}")]
    public IActionResult UpdateMeetup([FromRoute] Guid id, [FromBody] Meetup updatedMeetup)
    {
        var prevMeetup = Meetups.SingleOrDefault(meetup => meetup.Id == id);

        if (prevMeetup is null)
        {
            return NotFound();
        }

        prevMeetup.Topic = updatedMeetup.Topic;
        prevMeetup.Duration = updatedMeetup.Duration;
        prevMeetup.Place = updatedMeetup.Place;

        return NoContent();
    }

    [HttpGet]
    public IActionResult GetMeetups()
    {
        return Ok(Meetups);
    }

    [HttpDelete]
    public IActionResult DeleteMeetup([FromRoute] Guid id)
    {
        var meetupToDelete = Meetups.SingleOrDefault(meetup => meetup.Id == id);

        if (meetupToDelete is null)
        {
            return NotFound();
        }

        Meetups.Remove(meetupToDelete);
        return Ok(meetupToDelete);
    }

    [HttpPost]
    public IActionResult CreateMeetup([FromBody] Meetup newMeetup)
    {
        newMeetup.Id = Guid.NewGuid();
        Meetups.Add(newMeetup);
        return Ok(newMeetup);
    }
}
