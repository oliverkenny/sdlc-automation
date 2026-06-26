using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Components;
using TeamDashboard.Pages;
using TeamDashboard.Services;

namespace TeamDashboard.Tests;

public class ToastTests : TestContext
{
    [Fact]
    public void ToastService_Show_AddsToast()
    {
        using var service = new ToastService(entranceDelay: TimeSpan.Zero);

        service.Show("Alice Johnson changed status from Available to Away", "info");

        var toast = Assert.Single(service.Toasts);
        Assert.Equal("Alice Johnson changed status from Available to Away", toast.Message);
        Assert.Equal("info", toast.Type);
    }

    [Fact]
    public void ToastService_Show_KeepsOnlyLatestThreeToasts()
    {
        using var service = new ToastService(entranceDelay: TimeSpan.Zero);

        service.Show("Toast 1", "info");
        service.Show("Toast 2", "info");
        service.Show("Toast 3", "warning");
        service.Show("Toast 4", "success");

        Assert.Equal(3, service.Toasts.Count);
        Assert.DoesNotContain(service.Toasts, toast => toast.Message == "Toast 1");
        Assert.Contains(service.Toasts, toast => toast.Message == "Toast 4");
    }

    [Fact]
    public void ToastService_Dismiss_RemovesSpecificToast()
    {
        using var service = new ToastService(entranceDelay: TimeSpan.Zero, exitAnimationDelay: TimeSpan.Zero);

        service.Show("Toast 1", "info");
        service.Show("Toast 2", "warning");
        var toastToDismiss = service.Toasts.First(toast => toast.Message == "Toast 1");

        service.Dismiss(toastToDismiss.Id);

        Assert.Single(service.Toasts);
        Assert.DoesNotContain(service.Toasts, toast => toast.Id == toastToDismiss.Id);
    }

    [Fact]
    public void ToastContainer_RendersToasts()
    {
        var toastService = new FakeToastService();
        toastService.Show("Alice Johnson changed status from Available to Away", "info");
        Services.AddSingleton<IToastService>(toastService);

        var cut = RenderComponent<ToastContainer>();

        Assert.Contains("Alice Johnson changed status from Available to Away", cut.Markup);
        Assert.Single(cut.FindAll(".toast-item"));
    }

    [Fact]
    public void ToastContainer_DismissButton_RemovesToast()
    {
        var toastService = new FakeToastService();
        toastService.Show("Bob Smith changed status from In Meeting to Available", "success");
        Services.AddSingleton<IToastService>(toastService);

        var cut = RenderComponent<ToastContainer>();

        cut.Find(".toast-dismiss").Click();

        Assert.Empty(toastService.Toasts);
        Assert.Empty(cut.FindAll(".toast-item"));
    }

    [Fact]
    public void HomePage_StatusChange_ShowsToast()
    {
        var members = new List<Models.TeamMember>
        {
            new() { Id = 1, Name = "Alice Johnson", Role = "Tech Lead", Status = "Available" }
        };
        var projects = new List<Models.Project>
        {
            new() { Id = 1, Name = "Test Project", Status = "On Track", ProgressPercentage = 50, ProjectLead = "Alice Johnson" }
        };
        var toastService = new FakeToastService();

        Services.AddSingleton<ITeamMemberService>(new FakeTeamMemberService(members));
        Services.AddSingleton<IProjectService>(new FakeProjectService(projects));
        Services.AddSingleton<IToastService>(toastService);

        var cut = RenderComponent<Home>();

        cut.Find(".status-select").Change("Away");

        var toast = Assert.Single(toastService.Toasts);
        Assert.Equal("Alice Johnson changed status from Available to Away", toast.Message);
        Assert.Equal("info", toast.Type);
    }
}
