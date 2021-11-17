using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var meetups = new List<Meetup>();

app.MapGet("/get-string", () => Results.Ok("it's ok this is the way how it suppose to works"));

app.MapGet("/meetups", () => Results.Ok(meetups));

app.MapDelete("/meetups/{id:guid}", ([FromRoute] Guid id) =>
{
    var meetupToDelete = meetups.SingleOrDefault(meetup => meetup.Id == id);

    if (meetupToDelete is null)
    {
        return Results.NotFound();
    }

    meetups.Remove(meetupToDelete);
    return Results.Ok(meetupToDelete);
});

app.MapPost("/meetups", ([FromBody] Meetup newMeetup) =>
{
    newMeetup.Id = Guid.NewGuid();
    meetups.Add(newMeetup);
    return Results.Ok(newMeetup);
});

app.MapPut("/meetups/{id:guid}", ([FromRoute] Guid id, [FromBody] Meetup updatedMeetup) =>
{
    var prevMeetup = meetups.SingleOrDefault(meetup => meetup.Id == id);

    if (prevMeetup is null)
    {
        return Results.NotFound();
    }

    prevMeetup.Topic = updatedMeetup.Topic;
    prevMeetup.Duration = updatedMeetup.Duration;
    prevMeetup.Place = updatedMeetup.Place;

    return Results.NoContent();
});

app.Run();

class Meetup
{
    public Guid? Id { get; set; }
    public string Topic { get; set; }
    public string Place { get; set; }
    public int Duration { get; set; }
}
