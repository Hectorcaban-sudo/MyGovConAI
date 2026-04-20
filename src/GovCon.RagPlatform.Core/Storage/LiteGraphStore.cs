namespace GovCon.RagPlatform.Core.Storage;

public interface IGraphStore
{
    Task UpsertDocumentNodeAsync(string tenantId, string nodeId, string label, IReadOnlyDictionary<string, string> data, CancellationToken ct = default);
    Task UpsertEdgeAsync(string tenantId, string fromNodeId, string toNodeId, string edgeType, CancellationToken ct = default);
}

public sealed class LiteGraphStore : IGraphStore
{
    // Placeholder to wire LiteGraph client/server SDK from https://github.com/litegraphdb/litegraph
    public Task UpsertDocumentNodeAsync(string tenantId, string nodeId, string label, IReadOnlyDictionary<string, string> data, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task UpsertEdgeAsync(string tenantId, string fromNodeId, string toNodeId, string edgeType, CancellationToken ct = default)
        => Task.CompletedTask;
}
