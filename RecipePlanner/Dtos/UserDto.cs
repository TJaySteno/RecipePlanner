using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record UserDto (
    [Required] int UserID,
    [Required][StringLength(255)] string PrimaryEmail,
    [Required][StringLength(100)] string Username,
    [StringLength(100)] string? FirstName,
    [StringLength(100)] string? MiddleName,
    [StringLength(100)] string? LastName
);