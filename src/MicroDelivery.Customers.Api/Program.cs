using Dapr.Client;
using Dapr.Extensions.Configuration;
using MicroDelivery.Customers.Api.Data;
using MicroDelivery.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddDaprSecretStore(DaprConstants.LocalSecretStore, new DaprClientBuilder().Build(), new[] { ":" });

// Add services to the container.
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddHostedService<DatabaseSeeder>();

builder.Services.AddDbContext<CustomersContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServer");
    options
        .UseSqlServer(connectionString, opt => opt.EnableRetryOnFailure(3))
        .EnableDetailedErrors(true)
        .EnableSensitiveDataLogging(true);
});

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
