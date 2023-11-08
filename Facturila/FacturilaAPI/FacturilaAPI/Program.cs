using FacturilaAPI.Config;
using FacturilaAPI.Exceptions.Middleware;
using FacturilaAPI.Services;
using FacturilaAPI.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FacturilaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FacturilaConnectionString"));
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFirmService, FirmService>();

builder.Services.AddControllers();
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

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
