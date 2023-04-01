using Dapr.Client;
using Dapr.Extensions.Configuration;
using MicroDelivery.Orders.Api.Data;
using MicroDelivery.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddDaprSecretStore(DaprConstants.LocalSecretStore, new DaprClientBuilder().Build(), new[] { ":" });

// Add services to the container.
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();

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
