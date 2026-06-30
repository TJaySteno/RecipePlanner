using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Models;

// Todo: I don't think these are working.
[Index(nameof(PrimaryEmail), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int UserID { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public required string PrimaryEmail { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public required string Username { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(255)]
    public required string PasswordHash { private get; set; }

    [ScaffoldColumn(true)]
    [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string? FirstName { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string? MiddleName { get; set; }

    [ScaffoldColumn(true)]
    [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string? LastName { get; set; }
}
