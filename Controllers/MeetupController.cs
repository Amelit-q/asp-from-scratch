using Microsoft.AspNetCore.Mvc;

namespace AspFromScratch.WebApi.Controllers;

[ApiController]
[Route("/meetups")]
public class MeetupController : ControllerBase
{
    private static readonly ICollection<Meetup> Meetups = new List<Meetup>();

    public class Meetup
    {
        public Guid? Id { get; set; }
        public string Topic { get; set; }
        public string Place { get; set; }
        public int Duration { get; set; }
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
