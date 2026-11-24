using Microsoft.EntityFrameworkCore;
using TP1.Models;
using TP1.Data;
using TP1.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<RentalDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection2")));
var mongoSettings = builder.Configuration
    .GetSection("MongoDb")
    .Get<MongoDbSettings>()!;

builder.Services.AddSingleton(mongoSettings);
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ReviewService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var seedGenres = new Dictionary<int, string>
    {
        [1] = "Action",
        [2] = "Drame",
        [3] = "Comédie"
    };

    foreach (var seedGenre in seedGenres)
    {
        var genre = await db.Genres.FindAsync(seedGenre.Key);

        if (genre == null)
        {
            db.Genres.Add(new Genre
            {
                Id = seedGenre.Key,
                Name = seedGenre.Value
            });
        }
        else if (genre.Name != seedGenre.Value)
        {
            genre.Name = seedGenre.Value;
        }
    }

    await db.SaveChangesAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
