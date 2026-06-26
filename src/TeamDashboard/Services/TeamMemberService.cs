using TeamDashboard.Models;

namespace TeamDashboard.Services;

public interface ITeamMemberService
{
    List<TeamMember> GetAllMembers();
    void UpdateStatus(int id, string status);
}

public class TeamMemberService : ITeamMemberService
{
    private readonly List<TeamMember> _members = new()
    {
        new TeamMember { Id = 1, Name = "Alice Johnson", Role = "Tech Lead", Status = "Available" },
        new TeamMember { Id = 2, Name = "Bob Smith", Role = "Senior Developer", Status = "In Meeting" },
        new TeamMember { Id = 3, Name = "Carol Williams", Role = "UX Designer", Status = "Focussing" },
        new TeamMember { Id = 4, Name = "David Brown", Role = "Backend Developer", Status = "Available" },
        new TeamMember { Id = 5, Name = "Eva Martinez", Role = "QA Engineer", Status = "Away" }
    };

    public List<TeamMember> GetAllMembers() => _members;

    public void UpdateStatus(int id, string status)
    {
        var member = _members.FirstOrDefault(m => m.Id == id);
        if (member is not null)
        {
            member.Status = status;
        }
    }
}
