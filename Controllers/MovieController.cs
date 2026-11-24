
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;
using TP1.Data;
using TP1.Services;

namespace TP1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReviewService _reviewService;

        public MoviesController(ApplicationDbContext context, ReviewService reviewService)
        {
            _context = context;
            _reviewService = reviewService;
        }

        // GET: /Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .ToListAsync();

            return View(movies);
        }

        // GET: /Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var reviews = await _reviewService.GetByMovieIdAsync(movie.Id);

            return View(new MovieDetailsViewModel
            {
                Movie = movie,
                Reviews = reviews
            });
        }

        // GET: /Movies/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = new SelectList(
                await _context.Genres.ToListAsync(),
                "Id",
                "Name");

            return View();
        }

        // POST: /Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = new SelectList(
                    await _context.Genres.ToListAsync(),
                    "Id",
                    "Name",
                    movie.GenreId);

                return View(movie);
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
