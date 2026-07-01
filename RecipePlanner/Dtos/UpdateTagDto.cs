using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record UpdateTagDto (
    [Required][StringLength(50)] string DisplayValue,
    [Required][StringLength(50)] string Category
);