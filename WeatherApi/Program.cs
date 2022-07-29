using WeatherAPI.Interface;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IWeatherService>(x =>
    ActivatorUtilities.CreateInstance<WeatherService>(x, "https://api.openweathermap.org/data/3.0/onecall", "efc210918503632a4cd7520a5a29de32"));


builder.Services.AddControllers();

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
