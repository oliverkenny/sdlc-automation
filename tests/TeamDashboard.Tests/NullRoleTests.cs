using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Components;
using TeamDashboard.Models;
using TeamDashboard.Services;
using Xunit;

namespace TeamDashboard.Tests;

public class NullRoleTests : TestContext
{
    [Fact]
    public void TeamMemberCard_NullRole_DisplaysPlaceholder()
    {
        var member = new TeamMember { Id = 99, Name = "Test User", Role = null!, Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        Assert.Contains("No role assigned", cut.Markup);
    }

    [Fact]
    public void TeamMemberCard_EmptyRole_DisplaysPlaceholder()
    {
        var member = new TeamMember { Id = 99, Name = "Test User", Role = "", Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        Assert.Contains("No role assigned", cut.Markup);
    }

    [Fact]
    public void TeamMemberCard_WhitespaceRole_DisplaysPlaceholder()
    {
        var member = new TeamMember { Id = 99, Name = "Test User", Role = "   ", Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        Assert.Contains("No role assigned", cut.Markup);
    }

    [Fact]
    public void TeamMemberCard_ValidRole_DisplaysRole()
    {
        var member = new TeamMember { Id = 99, Name = "Test User", Role = "Developer", Status = "Available" };

        var cut = RenderComponent<TeamMemberCard>(parameters =>
            parameters.Add(p => p.Member, member));

        Assert.Contains("Developer", cut.Markup);
        Assert.DoesNotContain("No role assigned", cut.Markup);
    }
}
