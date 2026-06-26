using TeamDashboard.Models;

namespace TeamDashboard.Services;

public interface IProjectService
{
    List<Project> GetAllProjects();
}

public class ProjectService : IProjectService
{
    private readonly List<Project> _projects = new()
    {
        new Project { Id = 1, Name = "Customer Portal Redesign", Status = "On Track", ProgressPercentage = 72, ProjectLead = "Alice Johnson" },
        new Project { Id = 2, Name = "API Migration v3", Status = "At Risk", ProgressPercentage = 45, ProjectLead = "David Brown" },
        new Project { Id = 3, Name = "Mobile App Launch", Status = "On Track", ProgressPercentage = 88, ProjectLead = "Bob Smith" },
        new Project { Id = 4, Name = "Data Pipeline Refactor", Status = "Blocked", ProgressPercentage = 20, ProjectLead = "Eva Martinez" }
    };

    public List<Project> GetAllProjects() => _projects;
}
