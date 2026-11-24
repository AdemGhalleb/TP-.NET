using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP1.Models;
using TP1.Migrations;
using TP1.Data;

public class GenreController : Controller
{
private readonly ApplicationDbContext _db;
public GenreController(ApplicationDbContext db) => _db = db;
public async Task<IActionResult> Index()
=> View(await _db.Genres.AsNoTracking().ToListAsync());
public IActionResult Create() => View();
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Name")] Genre genre)
{
if (!ModelState.IsValid) return View(genre);
_db.Genres.Add(genre);
await _db.SaveChangesAsync();
return RedirectToAction(nameof(Index));

}
}