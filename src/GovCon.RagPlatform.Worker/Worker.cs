using GovCon.RagPlatform.Core.Ingestion;
using GovCon.RagPlatform.Core.Models;

namespace GovCon.RagPlatform.Worker;

public sealed class DistributedIngestionWorker
{
    private readonly IngestionPipeline _pipeline;

    public DistributedIngestionWorker(IngestionPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    // Intended to be called by Wolverine MQTT message handlers.
    public Task HandleAsync(IngestionEnvelope envelope, CancellationToken ct = default)
        => _pipeline.ProcessAsync(envelope, ct);
}

public sealed class ReconciliationJob
{
    private readonly IEnumerable<IDataSourceConnector> _connectors;
    private readonly IngestionPipeline _pipeline;

    public ReconciliationJob(IEnumerable<IDataSourceConnector> connectors, IngestionPipeline pipeline)
    {
        _connectors = connectors;
        _pipeline = pipeline;
    }

    // Backup for missed webhooks.
    public async Task RunAsync(string tenantId, DateTimeOffset sinceUtc, CancellationToken ct = default)
    {
        foreach (var connector in _connectors)
        {
            await foreach (var doc in connector.GetChangedDocumentsAsync(sinceUtc, ct))
            {
                await _pipeline.ProcessAsync(new IngestionEnvelope(
                    tenantId,
                    Guid.NewGuid().ToString("N"),
                    doc,
                    DateTimeOffset.UtcNow), ct);
            }
        }
    }
}
