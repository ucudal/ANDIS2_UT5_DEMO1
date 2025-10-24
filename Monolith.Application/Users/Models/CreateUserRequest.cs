using System.ComponentModel.DataAnnotations;

namespace Monolith.Application.Users.Models;

public class CreateUserRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;
}
