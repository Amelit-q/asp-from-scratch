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

    [HttpGet]
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

    [HttpDelete("{id:guid}")]
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
