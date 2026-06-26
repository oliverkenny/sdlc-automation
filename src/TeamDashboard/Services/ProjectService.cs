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

    public ProjectService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        _projects ??= await _httpClient.GetFromJsonAsync<List<Project>>("data/projects.json") ?? new();
        return _projects;
    }
}
