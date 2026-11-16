using Hamac.Api.Data;
using Hamac.Api.Policies;
using Hamac.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var frontOrigins = builder.Configuration.GetSection("Front:Origins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy("Front", policy => policy
        .WithOrigins(frontOrigins)
        .WithMethods("GET", "POST")
        .WithHeaders("Content-Type")));

builder.Services.AddDbContext<HamacDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Hamac")));

builder.Services.AddSingleton<ILeaveEntitlementPolicy, PermanentContractPolicy>();
builder.Services.AddSingleton<ILeaveEntitlementPolicy, FixedTermContractPolicy>();
builder.Services.AddSingleton<ILeaveEntitlementPolicy, ApprenticeshipPolicy>();
builder.Services.AddSingleton<ILeaveEntitlementPolicy, InternshipPolicy>();
builder.Services.AddSingleton<LeavePolicyResolver>();

builder.Services.AddScoped<ILeaveNotifier, NotificationService>();
builder.Services.AddScoped<LeaveService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HamacDbContext>();
    DemoData.Reset(db);
}

app.UseExceptionHandler();
app.UseCors("Front");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
