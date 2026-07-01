using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record CreateRecipeTagDto (
    [Required] int TagID
);