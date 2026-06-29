using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record CreateUserDto(
    [Required][StringLength(255)] string PrimaryEmail,
    [Required][StringLength(100)] string Username,
    [Required][StringLength(255)] string PasswordHash,
    [StringLength(100)] string? FirstName,
    [StringLength(100)] string? MiddleName,
    [StringLength(100)] string? LastName
);