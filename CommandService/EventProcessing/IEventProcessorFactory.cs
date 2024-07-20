using CommandService.EventProcessing;

public interface IEventProcessorFactory
{
    IEventProcessor CreateEventProcessor();
}
