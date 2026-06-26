using System.Net.Http.Json;
using TeamDashboard.Models;

namespace TeamDashboard.Services;

public interface IProjectService
{
    Task<List<Project>> GetAllProjectsAsync();
}

public class ProjectService : IProjectService
{
    private readonly HttpClient _httpClient;
    private List<Project>? _projects;
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

    public ProjectService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        if (_projects is not null)
        {
            return _projects;
        }

        using var cancellationTokenSource = new CancellationTokenSource(RequestTimeout);

        try
        {
            _projects = await _httpClient.GetFromJsonAsync<List<Project>>("data/projects.json", cancellationTokenSource.Token) ?? new();
        }
        catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
        {
            throw new TimeoutException("Loading projects timed out after 5 seconds. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("We couldn't load projects right now. Please try again.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new InvalidOperationException("We couldn't read the project data. Please try again.", ex);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidOperationException("We couldn't read the project data. Please try again.", ex);
        }

        return _projects;
    }
}
