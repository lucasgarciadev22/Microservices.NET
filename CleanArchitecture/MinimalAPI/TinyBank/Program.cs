using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TinyBank.Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<BankContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"))
);

ApiMapperHelper.MapEndpoints(app);

app.Run();
