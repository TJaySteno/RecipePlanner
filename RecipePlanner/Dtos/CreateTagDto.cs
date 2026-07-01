using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record CreateTagDto (
    [Required][StringLength(50)] string DisplayValue,
    [Required][StringLength(50)] string Category
);