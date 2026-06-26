using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Components;
using TeamDashboard.Models;
using TeamDashboard.Pages;
using TeamDashboard.Services;
using Xunit;

namespace TeamDashboard.Tests;

public class StatusToggleTests : TestContext
{
    private void RegisterServices()
    {
        Services.AddSingleton<ITeamMemberService, TeamMemberService>();
        Services.AddSingleton<IProjectService, ProjectService>();
    }

    [Fact]
    public void TeamMemberCard_HasStatusDropdown()
    {
        var member = new TeamMember { Id = 1, Name = "Test User", Role = "Dev", Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        var select = cut.Find(".status-select");
        Assert.NotNull(select);
    }

    [Fact]
    public void TeamMemberCard_DropdownHasAllStatusOptions()
    {
        var member = new TeamMember { Id = 1, Name = "Test User", Role = "Dev", Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        var options = cut.FindAll(".status-select option");
        Assert.Equal(4, options.Count);
        Assert.Contains(options, o => o.TextContent == "Available");
        Assert.Contains(options, o => o.TextContent == "In Meeting");
        Assert.Contains(options, o => o.TextContent == "Focussing");
        Assert.Contains(options, o => o.TextContent == "Away");
    }

    [Fact]
    public void TeamMemberCard_StatusChange_UpdatesMember()
    {
        var member = new TeamMember { Id = 1, Name = "Test User", Role = "Dev", Status = "Available" };
        var receivedStatus = "";

        var cut = RenderComponent<TeamMemberCard>(parameters => parameters
            .Add(p => p.Member, member)
            .Add(p => p.OnStatusChange, (string s) => receivedStatus = s));

        var select = cut.Find(".status-select");
        select.Change("Away");

        Assert.Equal("Away", member.Status);
        Assert.Equal("Away", receivedStatus);
    }

    [Fact]
    public void TeamMemberCard_StatusChange_UpdatesIndicatorClass()
    {
        var member = new TeamMember { Id = 1, Name = "Test User", Role = "Dev", Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        var indicator = cut.Find(".status-indicator");
        Assert.Contains("status-available", indicator.ClassList);

        cut.Find(".status-select").Change("In Meeting");

        indicator = cut.Find(".status-indicator");
        Assert.Contains("status-meeting", indicator.ClassList);
    }

    [Fact]
    public void HomePage_StatusChange_PersistsInService()
    {
        RegisterServices();

        var cut = RenderComponent<Home>();

        var selects = cut.FindAll(".status-select");
        Assert.True(selects.Count > 0);

        selects[0].Change("Away");

        var service = Services.GetRequiredService<ITeamMemberService>();
        var members = service.GetAllMembers();
        Assert.Equal("Away", members[0].Status);
    }
}
