using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Models;
using TeamDashboard.Pages;
using TeamDashboard.Services;
using Xunit;

namespace TeamDashboard.Tests;

public class ProjectSectionTests : TestContext
{
    [Fact]
    public void HomePage_DisplaysProjectCards()
    {
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();

        var cut = RenderComponent<Home>();

        var cards = cut.FindAll(".project-card");
        Assert.True(cards.Count >= 3);
    }

    [Fact]
    public void HomePage_DisplaysProjectNames()
    {
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("Customer Portal Redesign", markup);
        Assert.Contains("API Migration v3", markup);
        Assert.Contains("Mobile App Launch", markup);
    }

    [Fact]
    public void HomePage_DisplaysProgressBars()
    {
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();

        var cut = RenderComponent<Home>();

        var progressBars = cut.FindAll(".progress-bar");
        Assert.True(progressBars.Count >= 3);
    }

    [Fact]
    public void HomePage_DisplaysStatusLabels()
    {
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("On Track", markup);
        Assert.Contains("At Risk", markup);
        Assert.Contains("Blocked", markup);
    }

    [Fact]
    public void HomePage_DisplaysProjectLeads()
    {
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();

        var cut = RenderComponent<Home>();

        var markup = cut.Markup;
        Assert.Contains("Alice Johnson", markup);
        Assert.Contains("David Brown", markup);
    }
}
