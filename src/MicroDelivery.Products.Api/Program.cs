using MicroDelivery.Products.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddHostedService<DatabaseSeeder>();

builder.Services.AddDbContext<ProductsContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
    options
        .UseNpgsql(connectionString, opt => opt.EnableRetryOnFailure(3))
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
