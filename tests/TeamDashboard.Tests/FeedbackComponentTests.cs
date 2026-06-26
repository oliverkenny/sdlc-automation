using Bunit;
using Microsoft.AspNetCore.Components;
using TeamDashboard.Components;

namespace TeamDashboard.Tests;

public class FeedbackComponentTests : TestContext
{
    [Fact]
    public void LoadingSpinner_RendersDefaultMessage()
    {
        var cut = RenderComponent<LoadingSpinner>();

        Assert.Contains("Loading...", cut.Markup);
        Assert.NotNull(cut.Find(".spinner-circle"));
    }

    [Fact]
    public void ErrorDisplay_RendersMessageAndRetryButton()
    {
        var cut = RenderComponent<ErrorDisplay>(parameters => parameters
            .Add(p => p.Message, "Something has gone wrong."));

        Assert.Contains("Something has gone wrong.", cut.Markup);
        Assert.Equal("Try Again", cut.Find(".retry-button").TextContent.Trim());
    }

    [Fact]
    public void ErrorDisplay_InvokesRetryCallback()
    {
        var retried = false;

        var cut = RenderComponent<ErrorDisplay>(parameters => parameters
            .Add(p => p.Message, "Something has gone wrong.")
            .Add(p => p.OnRetry, EventCallback.Factory.Create(this, () => retried = true)));

        cut.Find(".retry-button").Click();

        Assert.True(retried);
    }
}
