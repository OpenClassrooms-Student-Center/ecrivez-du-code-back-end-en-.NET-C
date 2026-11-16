using Hamac.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

using (var db = new HamacDbContext())
{
    DemoData.Reset(db);
}

app.MapControllers();

app.Run();
