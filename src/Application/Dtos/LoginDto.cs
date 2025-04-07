using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class LoginDto
{
    [Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}