using Dapr.Client;
using Dapr.Extensions.Configuration;
using MicroDelivery.Discount.Api;
using MicroDelivery.Shared;

var builder = WebApplication.CreateBuilder(args);

var client = new DaprClientBuilder().Build();
builder.Configuration
    .AddDaprConfigurationStore(DaprConstants.RedisConfigStore, new List<string>() { DiscountConstants.CrazyDiscountEnabledKey }, client, TimeSpan.FromSeconds(20))
    .AddStreamingDaprConfigurationStore(DaprConstants.RedisConfigStore, new List<string>() { DiscountConstants.CrazyDiscountEnabledKey }, client, TimeSpan.FromSeconds(20));

// Add services to the container.

builder.Services.AddControllers().AddDapr();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.MapSubscribeHandler();
app.UseCloudEvents();

app.Run();
