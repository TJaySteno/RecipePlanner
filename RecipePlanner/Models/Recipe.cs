namespace RecipePlanner.Models;

public class Recipe
{
    public int RecipeID { get; set; }
    public required int OwnerID { get; set; } // Todo: Change this to "User Owner" later on. Will need to update all Recipe instances.
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public float Rating { get; set; }
    public int Difficulty { get; set; }
    public int PrepTimeInMinutes { get; set; }
    public int CookTimeInMinutes { get; set; }
    public int CoolTimeInMinutes { get; set; }
    public int Servings { get; set; }
    public int Calories { get; set; }
    public int ProteinInGrams { get; set; }
    public int CarbsInGrams { get; set; }
    public int FatInGrams { get; set; }
}
