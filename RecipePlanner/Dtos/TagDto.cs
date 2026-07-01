using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record TagDto (
    [Required] int TagID,
    [Required][StringLength(50)] string DisplayValue,
    [Required][StringLength(50)] string Category,
    int CreatedBy,
    int ModifiedBy
);