using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TinyBank.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"))
);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.InitializeDatabase();
app.MapMinimalEndpoints();

app.Run();
