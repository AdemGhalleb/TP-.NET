using Microsoft.AspNetCore.Mvc;
using TP1.Data;
using TP1.Models;
using TP1.Services;

namespace TP1.Controllers;

public class ReviewsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ReviewService _reviewService;

    public ReviewsController(ApplicationDbContext db, ReviewService reviewService)
    {
        _db = db;
        _reviewService = reviewService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        int movieId,
        [Bind(Prefix = "ReviewForm")] ReviewInputModel input)
    {
        if (await _db.Movies.FindAsync(movieId) == null)
        {
            return NotFound("The movie could not be found.");
        }

        if (!ModelState.IsValid)
        {
            var movie = await _db.Movies.FindAsync(movieId);
            if (movie == null)
            {
                return NotFound("The movie could not be found.");
            }

            var reviews = await _reviewService.GetByMovieIdAsync(movieId);
            ViewData["ReviewError"] = "Please correct the review errors.";
            return View("../Movies/Details", new MovieDetailsViewModel
            {
                Movie = movie,
                Reviews = reviews,
                ReviewForm = input
            });
        }

        await _reviewService.CreateAsync(new Review
        {
            MovieId = movieId,
            CustomerName = input.CustomerName.Trim(),
            Rating = input.Rating,
            Comment = input.Comment.Trim()
        });

        TempData["Success"] = "Your review was added.";
        return RedirectToAction("Details", "Movies", new { id = movieId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var review = await _reviewService.GetByIdAsync(id);
        if (review == null)
        {
            return NotFound("The review could not be found.");
        }

        var movie = await _db.Movies.FindAsync(review.MovieId);
        if (movie == null)
        {
            return NotFound("The reviewed movie could not be found.");
        }

        return View(new ReviewEditViewModel
        {
            Id = id,
            MovieId = review.MovieId,
            MovieName = movie.Name,
            CustomerName = review.CustomerName,
            Rating = review.Rating,
            Comment = review.Comment
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ReviewEditViewModel input)
    {
        var review = await _reviewService.GetByIdAsync(input.Id);
        if (review == null)
        {
            return NotFound("The review could not be found.");
        }

        if (!ModelState.IsValid)
        {
            var movie = await _db.Movies.FindAsync(review.MovieId);
            input.MovieName = movie?.Name;
            input.MovieId = review.MovieId;
            return View(input);
        }

        await _reviewService.UpdateAsync(
            input.Id,
            input.CustomerName.Trim(),
            input.Rating,
            input.Comment.Trim());

        TempData["Success"] = "The review was updated.";
        return RedirectToAction("Details", "Movies", new { id = review.MovieId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var review = await _reviewService.GetByIdAsync(id);
        if (review == null)
        {
            return NotFound("The review could not be found.");
        }

        var deleted = await _reviewService.DeleteAsync(id);
        if (!deleted)
        {
            TempData["Error"] = "The review could not be deleted.";
            return RedirectToAction("Details", "Movies", new { id = review.MovieId });
        }

        TempData["Success"] = "The review was deleted.";
        return RedirectToAction("Details", "Movies", new { id = review.MovieId });
    }
}
