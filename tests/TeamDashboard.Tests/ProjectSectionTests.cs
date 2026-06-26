using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Models;
using TeamDashboard.Pages;
using TeamDashboard.Services;
using Xunit;

namespace TeamDashboard.Tests;

public class ProjectSectionTests : TestContext
{
    private void RegisterServices()
    {
        var members = new List<TeamMember>
        {
            new() { Id = 1, Name = "Alice Johnson", Role = "Tech Lead", Status = "Available" }
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
    }

    [Fact]
    public void HomePage_DisplaysProjectCards()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var cards = cut.FindAll(".project-card");
        Assert.True(cards.Count >= 3);
    }

    [Fact]
    public void HomePage_DisplaysProjectNames()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("Customer Portal Redesign", markup);
        Assert.Contains("API Migration v3", markup);
        Assert.Contains("Mobile App Launch", markup);
    }

    [Fact]
    public void HomePage_DisplaysProgressBars()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var progressBars = cut.FindAll(".progress-bar");
        Assert.True(progressBars.Count >= 3);
    }

    [Fact]
    public void HomePage_DisplaysStatusLabels()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("On Track", markup);
        Assert.Contains("At Risk", markup);
        Assert.Contains("Blocked", markup);
    }

    [Fact]
    public void HomePage_DisplaysProjectLeads()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("Alice Johnson", markup);
        Assert.Contains("David Brown", markup);
    }
}
