using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record RecipeTagSummaryDto (
    [Required] int RecipeID,
    string RecipeName,
    [Required] List<TagSummaryDto> Tags
);
