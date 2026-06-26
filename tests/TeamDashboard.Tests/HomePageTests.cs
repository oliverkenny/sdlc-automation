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
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();
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
