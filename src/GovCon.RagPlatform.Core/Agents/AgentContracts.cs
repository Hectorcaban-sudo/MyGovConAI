using GovCon.RagPlatform.Core.Models;

namespace GovCon.RagPlatform.Core.Agents;

public sealed record AgentRequest(string TenantId, string UserId, string Query, IReadOnlyDictionary<string, string>? Context = null);
public sealed record AgentResponse(string AgentName, string Output, IReadOnlyList<RetrievalResult> Citations);

public interface IGovConAgent
{
    string Name { get; }
    Task<AgentResponse> ExecuteAsync(AgentRequest request, CancellationToken ct = default);
}
