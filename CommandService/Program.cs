using CommandService.Data;
using CommandService.EventProcessing;
using CommandService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IPlatformDataClient,PlatformDataClient>();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemo"));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddScoped<IEventProcessor,EventProcessor>();
builder.Services.AddScoped<IEventProcessorFactory, EventProcessorFactory>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// builder.Services.AddHostedService<MessageBusSubscriber>();
// builder.Services.AddSingleton<MessageBusSubscriber>();
var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();


PrepDb.PrepPopulation(app);
app.Run();
