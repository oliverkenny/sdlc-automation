namespace TeamDashboard.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "On Track";
    public int ProgressPercentage { get; set; }
    public string ProjectLead { get; set; } = string.Empty;
}
