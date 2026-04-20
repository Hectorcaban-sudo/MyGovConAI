using GovCon.RagPlatform.Core.Agents;

namespace GovCon.RagPlatform.Core.Orchestration;

public sealed class AgentOrchestrator
{
    private readonly Dictionary<string, IGovConAgent> _agents;

    public AgentOrchestrator(IEnumerable<IGovConAgent> agents)
    {
        _agents = agents.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
    }

    public Task<AgentResponse> RouteAsync(AgentRequest request, CancellationToken ct = default)
    {
        var target = SelectAgent(request.Query);
        return _agents[target].ExecuteAsync(request, ct);
    }

    private string SelectAgent(string query)
    {
        var q = query.ToLowerInvariant();
        if (q.Contains("past performance") || q.Contains("cpars")) return "past-performance-agent";
        if (q.Contains("rfp") || q.Contains("proposal")) return "rfp-proposal-matching-agent";
        if (q.Contains("competitor") || q.Contains("win probability")) return "competitor-analysis-agent";
        if (q.Contains("account") || q.Contains("customer")) return "accounts-agent";
        if (q.Contains("contract") || q.Contains("far") || q.Contains("clause")) return "contracts-agent";
        if (q.Contains("ops") || q.Contains("delivery") || q.Contains("sla")) return "ops-agent";
        return "performance-agent";
    }
}
