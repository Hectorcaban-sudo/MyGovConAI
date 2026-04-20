using GovCon.RagPlatform.Core.Models;

namespace SharpCoreDb.VectorSearch;

public interface IVectorEmbedder
{
    Task<float[]> EmbedAsync(string text, CancellationToken ct = default);
}

public interface ISharpCoreVectorStore
{
    Task UpsertChunkAsync(
        string tenantId,
        string chunkId,
        float[] embedding,
        string text,
        IReadOnlyDictionary<string, string> metadata,
        CancellationToken ct = default);

    Task<IReadOnlyList<RetrievalResult>> SearchAsync(
        string tenantId,
        float[] queryEmbedding,
        int topK,
        IReadOnlyDictionary<string, string>? filters,
        CancellationToken ct = default);
}

public static class SharpCoreVectorSearchExtensions
{
    // Extension requested: chunking + ingestion pipeline support with metadata preservation.
    public static async Task UpsertDocumentChunksAsync(
        this ISharpCoreVectorStore store,
        string tenantId,
        IEnumerable<DocumentChunk> chunks,
        IVectorEmbedder embedder,
        CancellationToken ct = default)
    {
        foreach (var chunk in chunks)
        {
            var embedding = await embedder.EmbedAsync(chunk.Text, ct);
            await store.UpsertChunkAsync(tenantId, chunk.ChunkId, embedding, chunk.Text, chunk.Metadata, ct);
        }
    }
}
