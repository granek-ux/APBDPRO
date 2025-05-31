using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Konfiguracja kontekstu bazy danych
// ConnectionString jest pobierany z appsettings.json, oczywiście należy go tam też ustawić
// builder.Services.AddDbContext<ApbdContext>(options => 
    // options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"))
// );

// Wstrzykiwanie zależności
// https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
// builder.Services.AddScoped<ITripsService, TripsService>();
// builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.DocExpansion(DocExpansion.List);
        c.DefaultModelExpandDepth(0);
        c.DisplayRequestDuration();
        c.EnableFilter();
    });
}

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();