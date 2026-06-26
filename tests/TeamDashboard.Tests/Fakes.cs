using TeamDashboard.Models;
using TeamDashboard.Services;

namespace TeamDashboard.Tests;

public class FakeTeamMemberService : ITeamMemberService
{
    private readonly List<TeamMember> _members;

    public FakeTeamMemberService(List<TeamMember> members)
    {
        _members = members;
    }

    public Task<List<TeamMember>> GetAllMembersAsync() => Task.FromResult(_members);

    public void UpdateStatus(int id, string status)
    {
        var member = _members.FirstOrDefault(m => m.Id == id);
        if (member is not null)
        {
            member.Status = status;
        }
    }
}

public class FakeProjectService : IProjectService
{
    private readonly List<Project> _projects;

    public FakeProjectService(List<Project> projects)
    {
        _projects = projects;
    }

    public Task<List<Project>> GetAllProjectsAsync() => Task.FromResult(_projects);
}

public class FakeToastService : IToastService
{
    private readonly List<Toast> _toasts = [];

    public event Action? OnChange;

    public IReadOnlyList<Toast> Toasts => _toasts.AsReadOnly();

    public void Show(string message, string type = "info")
    {
        _toasts.Add(new Toast
        {
            Message = message,
            Type = type,
            IsVisible = true
        });
        OnChange?.Invoke();
    }

    public void Dismiss(Guid id)
    {
        var toast = _toasts.FirstOrDefault(item => item.Id == id);
        if (toast is null)
        {
            return;
        }

        _toasts.Remove(toast);
        OnChange?.Invoke();
    }
}
