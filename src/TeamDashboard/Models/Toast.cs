namespace TeamDashboard.Models;

public class Toast
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsVisible { get; set; } = true;
}
