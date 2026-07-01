using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Models;

[Index(nameof(DisplayValue), nameof(Category), IsUnique = true)]
public class Tag
{
    [Key]
    public int TagID { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public required string DisplayValue { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public required string Category { get; set; }

    public User? CreatedBy { get; set; } = null;
    public User? ModifiedBy { get; set; } = null;

    public ICollection<RecipeTag> RecipeTags { get; set; } = [];
}
