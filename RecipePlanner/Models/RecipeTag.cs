using Microsoft.EntityFrameworkCore;

namespace RecipePlanner.Models;

[PrimaryKey(nameof(Recipe), nameof(Tag))]
public class RecipeTag
{
    public int RecipeID { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public int TagID { get; set; }
    public Tag Tag { get; set; } = null!;
}
