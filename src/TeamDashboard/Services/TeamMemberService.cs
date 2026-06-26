using System.Net.Http.Json;
using TeamDashboard.Models;

namespace TeamDashboard.Services;

public interface ITeamMemberService
{
    Task<List<TeamMember>> GetAllMembersAsync();
    void UpdateStatus(int id, string status);
}

public class TeamMemberService : ITeamMemberService
{
    private readonly HttpClient _httpClient;
    private List<TeamMember>? _members;

    public TeamMemberService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TeamMember>> GetAllMembersAsync()
    {
        _members ??= await _httpClient.GetFromJsonAsync<List<TeamMember>>("data/team-members.json") ?? new();
        return _members;
    }

    public void UpdateStatus(int id, string status)
    {
        var member = _members?.FirstOrDefault(m => m.Id == id);
        if (member is not null)
        {
            member.Status = status;
        }
    }
}
