using RecipePlanner.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

// This class represents a data transfer object (DTO) for a recipe.
// A DTO is a contract between the client and server since it represents
// a shared agreement about how data wil be transferred and used.

public record RecipeDetailsDto (
    [Required] int RecipeID,
    [Required] int OwnerID,
    [Required][StringLength(50)] string Name,
    [StringLength(255)] string? Description,
    [StringLength(255)] string? Url,
    [Range(1.0, 5.0)] float? Rating,
    [Range(1, 5)] int? Difficulty,
    int? PrepTimeInMinutes,
    int? CookTimeInMinutes,
    int? CoolTimeInMinutes,
    int? Servings,
    int? Calories,
    int? ProteinInGrams,
    int? CarbsInGrams,
    int? FatInGrams
);