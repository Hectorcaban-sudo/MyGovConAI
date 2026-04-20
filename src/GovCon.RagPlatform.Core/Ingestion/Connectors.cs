using GovCon.RagPlatform.Core.Models;

namespace GovCon.RagPlatform.Core.Ingestion;

public sealed class SharePointConnector : IDataSourceConnector
{
    public string SourceType => "sharepoint";
    public async IAsyncEnumerable<SourceDocument> GetChangedDocumentsAsync(DateTimeOffset sinceUtc, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        await Task.CompletedTask;
        yield break;
    }
}

public sealed class SqlConnector : IDataSourceConnector
{
    public string SourceType => "database";
    public async IAsyncEnumerable<SourceDocument> GetChangedDocumentsAsync(DateTimeOffset sinceUtc, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        await Task.CompletedTask;
        yield break;
    }
}

public sealed class ExcelConnector : IDataSourceConnector
{
    public string SourceType => "excel";
    public async IAsyncEnumerable<SourceDocument> GetChangedDocumentsAsync(DateTimeOffset sinceUtc, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        await Task.CompletedTask;
        yield break;
    }
}

public sealed class CustomConnector : IDataSourceConnector
{
    public string SourceType => "custom";
    public async IAsyncEnumerable<SourceDocument> GetChangedDocumentsAsync(DateTimeOffset sinceUtc, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        await Task.CompletedTask;
        yield break;
    }
}
