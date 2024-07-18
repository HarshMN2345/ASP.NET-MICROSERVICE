using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformsProfiles:Profile
{
    public PlatformsProfiles()
    {
        CreateMap<Platform,PlatformReadDto>();
        CreateMap<PlatformCreateDto,Platform>();
    }
}
