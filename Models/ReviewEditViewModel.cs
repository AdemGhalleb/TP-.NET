using System.ComponentModel.DataAnnotations;

namespace TP1.Models;

public class ReviewEditViewModel : ReviewInputModel
{
    public string Id { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public string? MovieName { get; set; }
}
