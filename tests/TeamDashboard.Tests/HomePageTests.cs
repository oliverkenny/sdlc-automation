using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Models;
using TeamDashboard.Pages;
using TeamDashboard.Services;
using Xunit;

namespace TeamDashboard.Tests;

public class HomePageTests : TestContext
{
    private void RegisterServices()
    {
        var members = new List<TeamMember>
        {
            new() { Id = 1, Name = "Alice Johnson", Role = "Tech Lead", Status = "Available" },
            new() { Id = 2, Name = "Bob Smith", Role = "Senior Developer", Status = "In Meeting" },
            new() { Id = 3, Name = "Carol Williams", Role = "UX Designer", Status = "Focussing" },
            new() { Id = 4, Name = "David Brown", Role = "Backend Developer", Status = "Available" },
            new() { Id = 5, Name = "Eva Martinez", Role = "QA Engineer", Status = "Away" }
        };
        var projects = new List<Project>
        {
            new() { Id = 1, Name = "Customer Portal Redesign", Status = "On Track", ProgressPercentage = 72, ProjectLead = "Alice Johnson" },
            new() { Id = 2, Name = "API Migration v3", Status = "At Risk", ProgressPercentage = 45, ProjectLead = "David Brown" },
            new() { Id = 3, Name = "Mobile App Launch", Status = "On Track", ProgressPercentage = 88, ProjectLead = "Bob Smith" },
            new() { Id = 4, Name = "Data Pipeline Refactor", Status = "Blocked", ProgressPercentage = 20, ProjectLead = "Eva Martinez" }
        };
        Services.AddSingleton<ITeamMemberService>(new FakeTeamMemberService(members));
        Services.AddSingleton<IProjectService>(new FakeProjectService(projects));
        Services.AddSingleton<IToastService>(new FakeToastService());
    }

    [Fact]
    public void HomePage_DisplaysAllTeamMembers()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var cards = cut.FindAll(".team-card");
        Assert.Equal(5, cards.Count);
    }

    [Fact]
    public void HomePage_DisplaysMemberNames()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("Alice Johnson", markup);
        Assert.Contains("Bob Smith", markup);
        Assert.Contains("Carol Williams", markup);
        Assert.Contains("David Brown", markup);
    }

    [Fact]
    public void HomePage_DisplaysMemberRoles()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("Tech Lead", markup);
        Assert.Contains("Senior Developer", markup);
        Assert.Contains("UX Designer", markup);
    }

    [Fact]
    public void HomePage_HasResponsiveGrid()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var grid = cut.Find(".team-grid");
        Assert.NotNull(grid);
    }

}
