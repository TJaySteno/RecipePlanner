using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record RecipeTagDto (
    [Required] int RecipeID,
    [Required] int TagID
);