namespace RecipePlanner.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public required string PrimaryEmail { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
}
