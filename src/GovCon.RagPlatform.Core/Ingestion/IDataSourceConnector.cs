using GovCon.RagPlatform.Core.Models;

namespace GovCon.RagPlatform.Core.Ingestion;

public interface IDataSourceConnector
{
    string SourceType { get; }
    IAsyncEnumerable<SourceDocument> GetChangedDocumentsAsync(DateTimeOffset sinceUtc, CancellationToken ct = default);
}

public interface IRealtimeWebhookReceiver
{
    Task HandleWebhookAsync(string payload, string signature, CancellationToken ct = default);
}
