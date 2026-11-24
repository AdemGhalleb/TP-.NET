namespace TP1.Models;

public class MovieDetailsViewModel
{
    public Movie Movie { get; set; } = null!;
    public List<Review> Reviews { get; set; } = new();
    public ReviewInputModel ReviewForm { get; set; } = new();

    public double AverageRating =>
        Reviews.Count == 0
            ? 0
            : Reviews.Average(review => review.Rating);
}
