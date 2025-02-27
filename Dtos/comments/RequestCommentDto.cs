using System.ComponentModel.DataAnnotations;

namespace FinSharkMarket.Dtos.comments;

public class RequestCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
    [MaxLength(50, ErrorMessage = "Title must be at most 50 characters long")]
    public String Title { get; set; } = String.Empty;
    
    [Required]
    [MinLength(5, ErrorMessage = "Content must be at least 5 characters long")]
    [MaxLength(255, ErrorMessage = "Content must be at most 255 characters long")]
    public String Content { get; set; } = String.Empty;
}