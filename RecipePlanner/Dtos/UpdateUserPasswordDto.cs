using System.ComponentModel.DataAnnotations;

namespace RecipePlanner.Dtos;

public record UpdateUserPasswordDto (
    [Required][StringLength(255)] string PasswordHash
);