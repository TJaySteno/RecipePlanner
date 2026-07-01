using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record TagSummaryDto (
    [Required] int TagID,
    [Required][StringLength(50)] string DisplayValue,
    [Required][StringLength(50)] string Category
);