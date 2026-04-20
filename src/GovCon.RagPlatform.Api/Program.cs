using GovCon.RagPlatform.Core.Agents;
using GovCon.RagPlatform.Core.Audit;
using GovCon.RagPlatform.Core.Ingestion;
using GovCon.RagPlatform.Core.Monitoring;
using GovCon.RagPlatform.Core.Orchestration;
using GovCon.RagPlatform.Core.Storage;
using SharpCoreDb.VectorSearch;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IChunker, SlidingWindowChunker>();
builder.Services.AddSingleton<IAuditLogger, InMemoryAuditLogger>();
builder.Services.AddSingleton<IMetrics, NoOpMetrics>();
builder.Services.AddSingleton<IGraphStore, LiteGraphStore>();

// TODO: replace fake implementations with production OpenAI and SharpCoreDB bindings.
builder.Services.AddSingleton<ISharpCoreVectorStore, FakeSharpCoreVectorStore>();
builder.Services.AddSingleton<IVectorEmbedder, FakeOpenAiEmbedder>();

builder.Services.AddScoped<IngestionPipeline>();
builder.Services.AddScoped<IGovConAgent, AccountsAgent>();
builder.Services.AddScoped<IGovConAgent, ContractsAgent>();
builder.Services.AddScoped<IGovConAgent, OpsAgent>();
builder.Services.AddScoped<IGovConAgent, PastPerformanceReviewAgent>();
builder.Services.AddScoped<IGovConAgent, ProposalMatchingAgent>();
builder.Services.AddScoped<IGovConAgent, ProposalGenerationAgent>();
builder.Services.AddScoped<IGovConAgent, CompetitorAnalysisAgent>();
builder.Services.AddScoped<IGovConAgent, PerformanceAgent>();
builder.Services.AddScoped<AgentOrchestrator>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/webhooks/sharepoint", async (SharePointWebhookPayload payload, IngestionPipeline pipeline, CancellationToken ct) =>
{
    // Webhook-first ingestion.
    var doc = payload.ToSourceDocument();
    await pipeline.ProcessAsync(new GovCon.RagPlatform.Core.Models.IngestionEnvelope(
        TenantId: payload.TenantId,
        CorrelationId: Guid.NewGuid().ToString("N"),
        Document: doc,
        EnqueuedAtUtc: DateTimeOffset.UtcNow), ct);

    return Results.Accepted();
});

app.MapPost("/api/agents/query", async (AgentQueryDto dto, AgentOrchestrator orchestrator, CancellationToken ct) =>
{
    var response = await orchestrator.RouteAsync(new AgentRequest(dto.TenantId, dto.UserId, dto.Query, dto.Context), ct);
    return Results.Ok(response);
});

app.MapGet("/api/admin/health", () => Results.Ok(new { status = "ok", component = "GovCon RAG Platform" }));
app.MapGet("/api/admin/metrics", () => Results.Ok(new { message = "Connect to OpenTelemetry/Prometheus exporter." }));

app.Run();

public sealed record AgentQueryDto(string TenantId, string UserId, string Query, Dictionary<string, string>? Context);

public sealed record SharePointWebhookPayload(string TenantId, string SiteId, string FileId, string Title, string Content)
{
    public GovCon.RagPlatform.Core.Models.SourceDocument ToSourceDocument() => new(
        SourceId: $"sharepoint:{SiteId}",
        ExternalId: FileId,
        SourceType: "sharepoint",
        Title: Title,
        Content: Content,
        Metadata: new Dictionary<string, string>
        {
            ["siteId"] = SiteId,
            ["fileId"] = FileId
        },
        LastModifiedUtc: DateTimeOffset.UtcNow);
}

public sealed class FakeOpenAiEmbedder : IVectorEmbedder
{
    public Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        var vector = Enumerable.Range(0, 32).Select(i => (float)((text.Length + i) % 11) / 10f).ToArray();
        return Task.FromResult(vector);
    }
}

public sealed class FakeSharpCoreVectorStore : ISharpCoreVectorStore
{
    private readonly Dictionary<string, List<GovCon.RagPlatform.Core.Models.RetrievalResult>> _data = new();

    public Task UpsertChunkAsync(string tenantId, string chunkId, float[] embedding, string text, IReadOnlyDictionary<string, string> metadata, CancellationToken ct = default)
    {
        if (!_data.ContainsKey(tenantId)) _data[tenantId] = new();
        _data[tenantId].RemoveAll(x => x.ChunkId == chunkId);
        _data[tenantId].Add(new GovCon.RagPlatform.Core.Models.RetrievalResult(
            chunkId,
            metadata.TryGetValue("sourceType", out var sourceType) ? sourceType : "unknown",
            metadata.TryGetValue("externalId", out var externalId) ? externalId : chunkId,
            text,
            1.0,
            metadata));

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<GovCon.RagPlatform.Core.Models.RetrievalResult>> SearchAsync(string tenantId, float[] queryEmbedding, int topK, IReadOnlyDictionary<string, string>? filters, CancellationToken ct = default)
    {
        if (!_data.TryGetValue(tenantId, out var list)) return Task.FromResult((IReadOnlyList<GovCon.RagPlatform.Core.Models.RetrievalResult>)Array.Empty<GovCon.RagPlatform.Core.Models.RetrievalResult>());
        return Task.FromResult((IReadOnlyList<GovCon.RagPlatform.Core.Models.RetrievalResult>)list.Take(topK).ToList());
    }
}
