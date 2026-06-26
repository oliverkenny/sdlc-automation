namespace TeamDashboard.Models;

public class TeamMember
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = "Available";
    public string Initials => string.Join("", Name.Split(' ').Select(n => n[0])).ToUpperInvariant();
}
