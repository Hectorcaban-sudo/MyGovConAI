using GovCon.RagPlatform.Core.Audit;
using GovCon.RagPlatform.Core.Models;
using GovCon.RagPlatform.Core.Monitoring;
using GovCon.RagPlatform.Core.Storage;
using SharpCoreDb.VectorSearch;

namespace GovCon.RagPlatform.Core.Ingestion;

public sealed class IngestionPipeline
{
    private readonly IChunker _chunker;
    private readonly ISharpCoreVectorStore _vectorStore;
    private readonly IVectorEmbedder _embedder;
    private readonly IGraphStore _graphStore;
    private readonly IAuditLogger _audit;
    private readonly IMetrics _metrics;

    public IngestionPipeline(
        IChunker chunker,
        ISharpCoreVectorStore vectorStore,
        IVectorEmbedder embedder,
        IGraphStore graphStore,
        IAuditLogger audit,
        IMetrics metrics)
    {
        _chunker = chunker;
        _vectorStore = vectorStore;
        _embedder = embedder;
        _graphStore = graphStore;
        _audit = audit;
        _metrics = metrics;
    }

    public async Task ProcessAsync(IngestionEnvelope envelope, CancellationToken ct = default)
    {
        var chunks = _chunker.Chunk(envelope.Document);
        await _vectorStore.UpsertDocumentChunksAsync(envelope.TenantId, chunks, _embedder, ct);

        var nodeId = $"doc:{envelope.Document.SourceId}:{envelope.Document.ExternalId}";
        await _graphStore.UpsertDocumentNodeAsync(
            envelope.TenantId,
            nodeId,
            "Document",
            new Dictionary<string, string>(envelope.Document.Metadata)
            {
                ["title"] = envelope.Document.Title,
                ["sourceType"] = envelope.Document.SourceType
            },
            ct);

        _metrics.Increment("rag.ingestion.documents", ("source", envelope.Document.SourceType));
        _metrics.Observe("rag.ingestion.chunks_per_document", chunks.Count, ("source", envelope.Document.SourceType));

        await _audit.WriteAsync(new AuditEvent(
            envelope.TenantId,
            envelope.CorrelationId,
            "system.ingestion",
            "IngestDocument",
            envelope.Document.SourceType,
            envelope.Document.ExternalId,
            DateTimeOffset.UtcNow,
            new Dictionary<string, string>
            {
                ["chunks"] = chunks.Count.ToString(),
                ["title"] = envelope.Document.Title
            }), ct);
    }
}
