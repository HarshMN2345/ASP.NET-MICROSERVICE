using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController:ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataCient;
  public PlatformsController(IPlatformRepo repository,IMapper mapper,ICommandDataClient commandDataClient,IMessageBusClient messageBusClient){
      _repository=repository;
        _mapper=mapper;
        _commandDataCient=commandDataClient;
        _messageBusClient=messageBusClient;
  }
  [HttpGet]
  public ActionResult<IEnumerable<PlatformReadDto>>GetPlaforms(){
        Console.WriteLine("--> Getting Platforms-->");
        var platformItems=_repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
  }

  [HttpGet("{id}",Name = nameof(GetPlatformById))]
  public ActionResult<PlatformReadDto> GetPlatformById(int id){
    var platformItem=_repository.GetPlatformById(id);
    if (platformItem!=null){
      return Ok(_mapper.Map<PlatformReadDto>(platformItem));
    }
    return NotFound();
  }

  
  [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        
        _repository.CreatePlatform(platformModel);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
        try{
          await _commandDataCient.SendPlatformToCommand(platformReadDto);
        }
        catch(Exception Ex){
          Console.WriteLine($"--> Could not send synchronously: {Ex.Message}");     
        }
        //send Async 
        try{
          var platformPublishedDto=_mapper.Map<PlatformPublishedDto>(platformReadDto);
          platformPublishedDto.Event="Platform_Published";
          _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch(Exception Ex){
          Console.WriteLine($"--> Could not send asynchronously: {Ex.Message}");     
        }

        // Return 201 Created response with Location header pointing to the newly created resource
        return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
    }
}
