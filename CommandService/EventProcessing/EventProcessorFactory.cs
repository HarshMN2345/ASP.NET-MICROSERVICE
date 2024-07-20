using CommandService.EventProcessing;

public class EventProcessorFactory : IEventProcessorFactory
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessorFactory(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IEventProcessor CreateEventProcessor()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            return scope.ServiceProvider.GetRequiredService<IEventProcessor>();
        }
    }
}
