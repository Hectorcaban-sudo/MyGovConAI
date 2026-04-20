using GovCon.RagPlatform.Core.Models;

namespace GovCon.RagPlatform.Core.Ingestion;

public interface IChunker
{
    IReadOnlyList<DocumentChunk> Chunk(SourceDocument document);
}

public sealed class SlidingWindowChunker : IChunker
{
    private readonly int _chunkSize;
    private readonly int _overlap;

    public SlidingWindowChunker(int chunkSize = 1200, int overlap = 150)
    {
        _chunkSize = chunkSize;
        _overlap = overlap;
    }

    public IReadOnlyList<DocumentChunk> Chunk(SourceDocument document)
    {
        var text = document.Content ?? string.Empty;
        var chunks = new List<DocumentChunk>();
        if (string.IsNullOrWhiteSpace(text)) return chunks;

        var offset = 0;
        var order = 0;
        while (offset < text.Length)
        {
            var len = Math.Min(_chunkSize, text.Length - offset);
            var slice = text.Substring(offset, len);

            chunks.Add(new DocumentChunk(
                ChunkId: $"{document.SourceId}:{document.ExternalId}:{order}",
                SourceId: document.SourceId,
                ExternalId: document.ExternalId,
                ChunkOrder: order,
                Text: slice,
                Metadata: new Dictionary<string, string>(document.Metadata)
                {
                    ["title"] = document.Title,
                    ["sourceType"] = document.SourceType,
                    ["lastModifiedUtc"] = document.LastModifiedUtc.ToString("O")
                }));

            order++;
            if (offset + len >= text.Length) break;
            offset += Math.Max(1, _chunkSize - _overlap);
        }

        return chunks;
    }
}
