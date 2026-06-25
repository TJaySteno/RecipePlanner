using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record CreateRecipeDto(
    [Required] int UserID,
    [Required][StringLength(50)] string Name,
    [StringLength(50)] string Description,
    [StringLength(50)] string Url,
    [Range(1.0, 5.0)] float Rating,
    [Range(1, 5)] int Difficulty,
    int PrepTimeInMinutes,
    int CookTimeInMinutes,
    int CoolTimeInMinutes,
    int Servings,
    int Calories,
    int ProteinInGrams,
    int CarbsInGrams,
    int FatInGrams
);