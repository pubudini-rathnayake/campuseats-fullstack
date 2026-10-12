using CampusEats.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Railway supplies PORT. Locally, launchSettings/default Kestrel configuration is used.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port)) builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var configuredOrigins = builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:5173";
var origins = configuredOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));

// Without a database connection string, the API runs with demonstration data.
// With Neon configured, the API uses actual PostgreSQL tables and migration(s).
var connection = builder.Configuration.GetConnectionString("Default");
var useDatabase = !string.IsNullOrWhiteSpace(connection);
if (useDatabase) builder.Services.AddDbContext<CampusDbContext>(options => options.UseNpgsql(connection));

var app = builder.Build();
app.UseCors("Frontend");

if (useDatabase)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CampusDbContext>();
    db.Database.Migrate();
    if (!db.MenuItems.Any())
    {
        db.MenuItems.AddRange(DemoData.Items.Select(x => new MenuItem
        {
            Id = x.Id, Name = x.Name, Category = x.Category, Price = x.Price,
            Description = x.Description, Emoji = x.Emoji, Featured = x.Featured
        }));
        db.SaveChanges();
    }
}

app.MapGet("/health", () => Results.Ok(new { status = "ok", database = useDatabase ? "configured" : "demo" }));
if (useDatabase)
{
    app.MapGet("/api/menu", async (CampusDbContext db) =>
        Results.Ok(await db.MenuItems.AsNoTracking().OrderBy(x => x.Id).ToListAsync()));
    app.MapGet("/api/menu/{id:int}", async (int id, CampusDbContext db) =>
    {
        var item = await db.MenuItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? Results.NotFound(new { message = "Dish not found" }) : Results.Ok(item);
    });
}
else
{
    app.MapGet("/api/menu", () => Results.Ok(DemoData.Items));
    app.MapGet("/api/menu/{id:int}", (int id) =>
    {
        var item = DemoData.Items.FirstOrDefault(x => x.Id == id);
        return item is null ? Results.NotFound(new { message = "Dish not found" }) : Results.Ok(item);
    });
}
app.Run();
