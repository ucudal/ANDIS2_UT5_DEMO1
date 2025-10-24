// ANDIS2.Monolith.Domain/Users/User.cs
namespace Monolith.Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
