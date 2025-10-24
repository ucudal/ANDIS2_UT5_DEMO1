using System.ComponentModel.DataAnnotations;

namespace Monolith.Application.Users.Models;

public class UpdateUserRequest
{
    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;
}
