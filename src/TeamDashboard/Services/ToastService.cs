using System.Threading;
using TeamDashboard.Models;

namespace TeamDashboard.Services;

public class ToastService : IToastService, IDisposable
{
    private const int MaxVisibleToasts = 3;

    private readonly List<Toast> _toasts = [];
    private readonly Dictionary<Guid, Timer> _autoDismissTimers = [];
    private readonly Dictionary<Guid, Timer> _entranceTimers = [];
    private readonly Dictionary<Guid, Timer> _removalTimers = [];
    private readonly object _syncRoot = new();
    private readonly TimeSpan _autoDismissDelay;
    private readonly TimeSpan _entranceDelay;
    private readonly TimeSpan _exitAnimationDelay;

    public ToastService(
        TimeSpan? autoDismissDelay = null,
        TimeSpan? entranceDelay = null,
        TimeSpan? exitAnimationDelay = null)
    {
        _autoDismissDelay = autoDismissDelay ?? TimeSpan.FromSeconds(5);
        _entranceDelay = entranceDelay ?? TimeSpan.FromMilliseconds(25);
        _exitAnimationDelay = exitAnimationDelay ?? TimeSpan.FromMilliseconds(250);
    }

    public event Action? OnChange;

    public IReadOnlyList<Toast> Toasts
    {
        get
        {
            lock (_syncRoot)
            {
                return _toasts
                    .OrderBy(toast => toast.CreatedAt)
                    .ToList()
                    .AsReadOnly();
            }
        }
    }

    public void Show(string message, string type = "info")
    {
        lock (_syncRoot)
        {
            while (_toasts.Count >= MaxVisibleToasts)
            {
                var oldestToastId = _toasts
                    .OrderBy(toast => toast.CreatedAt)
                    .First()
                    .Id;

                RemoveToastInternal(oldestToastId);
            }

            var toast = new Toast
            {
                Message = message,
                Type = type,
                IsVisible = _entranceDelay == TimeSpan.Zero
            };

            _toasts.Add(toast);

            if (_entranceDelay > TimeSpan.Zero)
            {
                _entranceTimers[toast.Id] = new Timer(
                    _ => SetVisible(toast.Id),
                    null,
                    _entranceDelay,
                    Timeout.InfiniteTimeSpan);
            }

            _autoDismissTimers[toast.Id] = new Timer(
                _ => Dismiss(toast.Id),
                null,
                _autoDismissDelay,
                Timeout.InfiniteTimeSpan);
        }

        NotifyStateChanged();
    }

    public void Dismiss(Guid id)
    {
        var shouldNotify = false;

        lock (_syncRoot)
        {
            var toast = _toasts.FirstOrDefault(item => item.Id == id);
            if (toast is null)
            {
                return;
            }

            DisposeTimer(_entranceTimers, id);
            DisposeTimer(_autoDismissTimers, id);

            if (_exitAnimationDelay == TimeSpan.Zero)
            {
                shouldNotify = RemoveToastInternal(id);
            }
            else if (toast.IsVisible || !_removalTimers.ContainsKey(id))
            {
                toast.IsVisible = false;
                _removalTimers[id] = new Timer(
                    _ => RemoveToast(id),
                    null,
                    _exitAnimationDelay,
                    Timeout.InfiniteTimeSpan);
                shouldNotify = true;
            }
        }

        if (shouldNotify)
        {
            NotifyStateChanged();
        }
    }

    public void Dispose()
    {
        lock (_syncRoot)
        {
            DisposeTimers(_autoDismissTimers);
            DisposeTimers(_entranceTimers);
            DisposeTimers(_removalTimers);
            _toasts.Clear();
        }
    }

    private void SetVisible(Guid id)
    {
        var shouldNotify = false;

        lock (_syncRoot)
        {
            var toast = _toasts.FirstOrDefault(item => item.Id == id);
            if (toast is null || _removalTimers.ContainsKey(id))
            {
                DisposeTimer(_entranceTimers, id);
                return;
            }

            if (!toast.IsVisible)
            {
                toast.IsVisible = true;
                shouldNotify = true;
            }

            DisposeTimer(_entranceTimers, id);
        }

        if (shouldNotify)
        {
            NotifyStateChanged();
        }
    }

    private void RemoveToast(Guid id)
    {
        var shouldNotify = false;

        lock (_syncRoot)
        {
            shouldNotify = RemoveToastInternal(id);
        }

        if (shouldNotify)
        {
            NotifyStateChanged();
        }
    }

    private bool RemoveToastInternal(Guid id)
    {
        var toast = _toasts.FirstOrDefault(item => item.Id == id);
        if (toast is null)
        {
            return false;
        }

        _toasts.Remove(toast);
        DisposeTimer(_autoDismissTimers, id);
        DisposeTimer(_entranceTimers, id);
        DisposeTimer(_removalTimers, id);
        return true;
    }

    private static void DisposeTimer(Dictionary<Guid, Timer> timers, Guid id)
    {
        if (timers.Remove(id, out var timer))
        {
            timer.Dispose();
        }
    }

    private static void DisposeTimers(Dictionary<Guid, Timer> timers)
    {
        foreach (var timer in timers.Values)
        {
            timer.Dispose();
        }

        timers.Clear();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
