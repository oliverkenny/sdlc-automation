using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TeamDashboard.Layout;
using TeamDashboard.Services;

namespace TeamDashboard.Tests;

public class MainLayoutTests : TestContext
{
    [Fact]
    public void MainLayout_RendersBodyContent()
    {
        Services.AddSingleton<IToastService>(new FakeToastService());

        var cut = RenderComponent<LayoutView>(parameters => parameters
            .Add(p => p.Layout, typeof(MainLayout))
            .AddChildContent("<p class=\"body-content\">Dashboard body</p>"));

        Assert.Contains("Dashboard body", cut.Markup);
        Assert.NotNull(cut.Find(".content"));
    }
}
