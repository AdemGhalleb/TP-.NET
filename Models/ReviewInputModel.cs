using System.ComponentModel.DataAnnotations;

namespace TP1.Models;

public class ReviewInputModel
{
    [Required(ErrorMessage = "Reviewer name is required.")]
    [StringLength(100, ErrorMessage = "Reviewer name cannot exceed 100 characters.")]
    [Display(Name = "Reviewer name")]
    public string CustomerName { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Select a rating from 1 to 5.")]
    [Display(Name = "Rating")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "A review comment is required.")]
    [StringLength(1000, ErrorMessage = "Review comment cannot exceed 1000 characters.")]
    [Display(Name = "Comment")]
    public string Comment { get; set; } = string.Empty;
}
