using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;
public class MessageBusClient:IMessageBusClient{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private IModel _channel;
    public MessageBusClient(IConfiguration configuration){
        _configuration=configuration;
        var factory=new ConnectionFactory(){
            HostName=_configuration["RabbitMQHost"],
            Port=int.Parse(_configuration["RabbitMQPort"])

         };
         try{
                _connection=factory.CreateConnection();
                _channel=_connection.CreateModel();
                _channel.ExchangeDeclare(exchange:"trigger",type:ExchangeType.Fanout);
                _connection.ConnectionShutdown+=RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to Message Bus");
            }
            catch(Exception ex){
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            
         }
        }

    private void SendMessage(string message){
        var body=Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange:"trigger",
        routingKey:"",
        basicProperties:null,
        body:body);
        Console.WriteLine($"--> We have sent {message}");
    }
    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
         Console.WriteLine("RabbitMq Connection is lost");
    }
    public void Dispose(){
        Console.WriteLine("--> MessageBus Disposed");
        if(_channel.IsOpen){
            _channel.Close();
            _connection.Close();
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto){
        var message=JsonSerializer.Serialize(platformPublishedDto);
        if(_connection.IsOpen){
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            SendMessage(message);
        }
        else{
            Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
        }
      

    }

}