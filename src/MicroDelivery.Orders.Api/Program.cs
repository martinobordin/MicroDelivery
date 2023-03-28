var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEventStoreClient(new Uri(builder.Configuration.GetConnectionString("EventStore")));

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

app.Run();
