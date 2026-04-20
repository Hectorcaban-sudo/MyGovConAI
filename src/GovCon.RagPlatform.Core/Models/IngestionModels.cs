namespace GovCon.RagPlatform.Core.Models;

public sealed record SourceDocument(
    string SourceId,
    string ExternalId,
    string SourceType,
    string Title,
    string Content,
    IReadOnlyDictionary<string, string> Metadata,
    DateTimeOffset LastModifiedUtc);

public sealed record DocumentChunk(
    string ChunkId,
    string SourceId,
    string ExternalId,
    int ChunkOrder,
    string Text,
    IReadOnlyDictionary<string, string> Metadata);

public sealed record IngestionEnvelope(
    string TenantId,
    string CorrelationId,
    SourceDocument Document,
    DateTimeOffset EnqueuedAtUtc);

public sealed record RetrievalRequest(
    string TenantId,
    string AgentName,
    string Query,
    int TopK,
    IReadOnlyDictionary<string, string>? Filters = null);

public sealed record RetrievalResult(
    string ChunkId,
    string SourceType,
    string ExternalId,
    string Text,
    double Score,
    IReadOnlyDictionary<string, string> Metadata);
