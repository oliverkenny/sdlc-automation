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

    [Fact]
    public void HomePage_ShowsLoadingStateWhileDataLoads()
    {
        var membersTaskSource = new TaskCompletionSource<List<TeamMember>>(TaskCreationOptions.RunContinuationsAsynchronously);
        var projectsTaskSource = new TaskCompletionSource<List<Project>>(TaskCreationOptions.RunContinuationsAsynchronously);

        Services.AddSingleton<ITeamMemberService>(new DeferredTeamMemberService(membersTaskSource.Task));
        Services.AddSingleton<IProjectService>(new DeferredProjectService(projectsTaskSource.Task));
        Services.AddSingleton<IToastService>(new FakeToastService());

        var cut = RenderComponent<Home>();

        Assert.Equal(2, cut.FindAll(".loading-spinner").Count);

        membersTaskSource.SetResult(new List<TeamMember>
        {
            new() { Id = 1, Name = "Alice Johnson", Role = "Tech Lead", Status = "Available" }
        });
        projectsTaskSource.SetResult(new List<Project>
        {
            new() { Id = 1, Name = "Customer Portal Redesign", Status = "On Track", ProgressPercentage = 72, ProjectLead = "Alice Johnson" }
        });

        cut.WaitForAssertion(() =>
        {
            Assert.Single(cut.FindAll(".team-card"));
            Assert.Single(cut.FindAll(".project-card"));
            Assert.Empty(cut.FindAll(".loading-spinner"));
        });
    }

    [Fact]
    public void HomePage_ShowsErrorStateWhenTeamMembersFailToLoad()
    {
        Services.AddSingleton<ITeamMemberService>(new ThrowingTeamMemberService(new InvalidOperationException("We couldn't load team members right now. Please try again.")));
        Services.AddSingleton<IProjectService>(new FakeProjectService(new List<Project>
        {
            new() { Id = 1, Name = "Customer Portal Redesign", Status = "On Track", ProgressPercentage = 72, ProjectLead = "Alice Johnson" }
        }));
        Services.AddSingleton<IToastService>(new FakeToastService());

        var cut = RenderComponent<Home>();

        cut.WaitForAssertion(() =>
        {
            var errorDisplay = cut.Find(".team-section .error-display");
            Assert.Contains("We couldn't load team members right now. Please try again.", errorDisplay.TextContent);
            Assert.Equal("Try Again", cut.Find(".team-section .retry-button").TextContent.Trim());
            Assert.Single(cut.FindAll(".project-card"));
        });
    }

    [Fact]
    public void HomePage_ShowsTimeoutMessageWhenLoadTimesOut()
    {
        Services.AddSingleton<ITeamMemberService>(new ThrowingTeamMemberService(new TimeoutException("Loading team members timed out after 5 seconds. Please try again.")));
        Services.AddSingleton<IProjectService>(new FakeProjectService(new List<Project>
        {
            new() { Id = 1, Name = "Customer Portal Redesign", Status = "On Track", ProgressPercentage = 72, ProjectLead = "Alice Johnson" }
        }));
        Services.AddSingleton<IToastService>(new FakeToastService());

        var cut = RenderComponent<Home>();

        cut.WaitForAssertion(() =>
        {
            var errorDisplay = cut.Find(".team-section .error-display-timeout");
            Assert.Contains("Loading team members timed out after 5 seconds. Please try again.", errorDisplay.TextContent);
            Assert.Contains("The request timed out after 5 seconds.", errorDisplay.TextContent);
        });
    }
}
