namespace GovCon.RagPlatform.Core.Audit;

public sealed record AuditEvent(
    string TenantId,
    string CorrelationId,
    string Actor,
    string Action,
    string ResourceType,
    string ResourceId,
    DateTimeOffset TimestampUtc,
    IReadOnlyDictionary<string, string> Details);

public interface IAuditLogger
{
    Task WriteAsync(AuditEvent evt, CancellationToken ct = default);
}

public sealed class InMemoryAuditLogger : IAuditLogger
{
    private readonly List<AuditEvent> _events = new();
    public Task WriteAsync(AuditEvent evt, CancellationToken ct = default)
    {
        _events.Add(evt);
        return Task.CompletedTask;
    }

    public IReadOnlyList<AuditEvent> Snapshot() => _events;
}
