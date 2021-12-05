using AutoMapper;

namespace AspFromScratch.WebApi.Meetup;

internal class MeetupMappingProfile : Profile
{
    public MeetupMappingProfile()
    {
        CreateMap<MeetupEntity, ReadMeetupDto>();
        CreateMap<MeetupEntity, CreateMeetupDto>();
        CreateMap<MeetupEntity, UpdateMeetupDto>();
    }
}
