using GovCon.RagPlatform.Core.Audit;
using GovCon.RagPlatform.Core.Models;
using SharpCoreDb.VectorSearch;

namespace GovCon.RagPlatform.Core.Agents;

public abstract class RagAgentBase : IGovConAgent
{
    private readonly ISharpCoreVectorStore _vectorStore;
    private readonly IVectorEmbedder _embedder;
    private readonly IAuditLogger _audit;

    protected RagAgentBase(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit)
    {
        _vectorStore = vectorStore;
        _embedder = embedder;
        _audit = audit;
    }

    public abstract string Name { get; }

    protected abstract string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits);

    protected virtual Task<string> CompleteAsync(string prompt, CancellationToken ct)
        => Task.FromResult($"[OpenAI completion placeholder]\n{prompt}");

    public async Task<AgentResponse> ExecuteAsync(AgentRequest request, CancellationToken ct = default)
    {
        var embedding = await _embedder.EmbedAsync(request.Query, ct);
        var hits = await _vectorStore.SearchAsync(request.TenantId, embedding, topK: 8, request.Context, ct);
        var prompt = BuildPrompt(request, hits);
        var output = await CompleteAsync(prompt, ct);

        await _audit.WriteAsync(new AuditEvent(
            request.TenantId,
            Guid.NewGuid().ToString("N"),
            request.UserId,
            "AgentQuery",
            "Agent",
            Name,
            DateTimeOffset.UtcNow,
            new Dictionary<string, string> { ["query"] = request.Query } ), ct);

        return new AgentResponse(Name, output, hits);
    }
}
