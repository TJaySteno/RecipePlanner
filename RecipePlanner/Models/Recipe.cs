using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Models;

public class Recipe
{
    public int RecipeID { get; set; }

    public required User Owner { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public required string Name { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string? Description { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string? Url { get; set; }

    [ScaffoldColumn(true)]
    [Range(1.0, 5.0, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public float? Rating { get; set; }

    [ScaffoldColumn(true)]
    [Range(1, 5, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? Difficulty { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1440, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? PrepTimeInMinutes { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1440, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? CookTimeInMinutes { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1440, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? CoolTimeInMinutes { get; set; }

    [ScaffoldColumn(true)]
    [Range(1, 100, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? Servings { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1000, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? Calories { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1000, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? ProteinInGrams { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1000, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? CarbsInGrams { get; set; }

    [ScaffoldColumn(true)]
    [Range(0, 1000, ErrorMessage = "The {0} value must be between {1} and {2} (inclusive).")]
    public int? FatInGrams { get; set; }
}
