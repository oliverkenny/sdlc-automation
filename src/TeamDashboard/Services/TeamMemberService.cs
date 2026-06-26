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
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

    public TeamMemberService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TeamMember>> GetAllMembersAsync()
    {
        if (_members is not null)
        {
            return _members;
        }

        using var cancellationTokenSource = new CancellationTokenSource(RequestTimeout);

        try
        {
            _members = await _httpClient.GetFromJsonAsync<List<TeamMember>>("data/team-members.json", cancellationTokenSource.Token) ?? new();
        }
        catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
        {
            throw new TimeoutException("Loading team members timed out after 5 seconds. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("We couldn't load team members right now. Please try again.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new InvalidOperationException("We couldn't read the team member data. Please try again.", ex);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidOperationException("We couldn't read the team member data. Please try again.", ex);
        }

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
