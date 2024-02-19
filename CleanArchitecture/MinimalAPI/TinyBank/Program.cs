using Infrastructure.Context;
using TinyBank.Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<BankContext>();
ApiMapperHelper.MapEndpoints(app);

app.Run();
