using TeamDashboard.Models;

namespace TeamDashboard.Services;

public interface IToastService
{
    event Action? OnChange;

    IReadOnlyList<Toast> Toasts { get; }

    void Show(string message, string type = "info");

    void Dismiss(Guid id);
}
