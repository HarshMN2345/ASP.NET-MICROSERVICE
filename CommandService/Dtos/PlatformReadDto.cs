namespace CommandService.Dtos;
public class PlatformReadDto{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string? Name { get; set; }
    public ICollection<CommandReadDto> Commands {get;set;}
}