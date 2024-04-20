using ASh.Framework.EventBus.RabbitMQ.Extensions;
using Meeting.API.IntegrationEvents;
using Meeting.API.IntegrationEvents.Handlers;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddConsumer<ProductAddedEventHandler, ProductAdded>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
