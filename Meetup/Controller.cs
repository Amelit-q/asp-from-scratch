using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspFromScratch.WebApi.Meetup;

[ApiController]
[Route("/meetups")]
public class MeetupController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public MeetupController(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>Creates new meetup</summary>
    /// <response code="200">Meetup was created</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateMeetup([FromBody] CreateMeetupDto createDto)
    {
        var newMeetup = _mapper.Map<MeetupEntity>(createDto);
        _context.Meetups.Add(newMeetup);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<MeetupEntity>(newMeetup);
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
        var prevMeetup = await _context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);

        if (prevMeetup is null)
        {
            return NotFound();
        }

        _mapper.Map(updateDto, prevMeetup);
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
        //Pay attention at the line beneath, especially at he generic <ICollection> that's because of type of the meetups (List)
        var readDtos = _mapper.Map<ICollection<ReadMeetupDto>>(meetups);
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
        var meetupToDelete = await _context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);

        if (meetupToDelete is null)
        {
            return NotFound();
        }

        _context.Meetups.Remove(meetupToDelete);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<ReadMeetupDto>(meetupToDelete);
        return Ok(readDto);
    }
}
