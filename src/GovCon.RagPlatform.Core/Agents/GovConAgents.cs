using GovCon.RagPlatform.Core.Audit;
using GovCon.RagPlatform.Core.Models;
using SharpCoreDb.VectorSearch;

namespace GovCon.RagPlatform.Core.Agents;

public sealed class AccountsAgent : RagAgentBase
{
    public AccountsAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "accounts-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Answer account-related GovCon query: {request.Query}\nEvidence chunks: {hits.Count}";
}

public sealed class ContractsAgent : RagAgentBase
{
    public ContractsAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "contracts-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Answer FAR/contract query: {request.Query}\nEvidence chunks: {hits.Count}";
}

public sealed class OpsAgent : RagAgentBase
{
    public OpsAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "ops-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Answer operations query: {request.Query}\nEvidence chunks: {hits.Count}";
}

public sealed class PastPerformanceReviewAgent : RagAgentBase
{
    public PastPerformanceReviewAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "past-performance-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Generate CPARS-style past performance review for gov contractor. Query: {request.Query}. Include insights from all source types (SharePoint, DB, Excel, custom). Hits: {hits.Count}";
}

public sealed class ProposalMatchingAgent : RagAgentBase
{
    public ProposalMatchingAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "rfp-proposal-matching-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Match RFP requirements to existing proposal content and identify gaps. Query: {request.Query}. Hits: {hits.Count}";
}

public sealed class ProposalGenerationAgent : RagAgentBase
{
    public ProposalGenerationAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "proposal-generation-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Generate proposal sections (SOW + technical volume) with citations. Query: {request.Query}. Hits: {hits.Count}";
}

public sealed class CompetitorAnalysisAgent : RagAgentBase
{
    public CompetitorAnalysisAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "competitor-analysis-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Estimate likely bidders and win probability for opportunity. Query: {request.Query}. Hits: {hits.Count}";
}

public sealed class PerformanceAgent : RagAgentBase
{
    public PerformanceAgent(ISharpCoreVectorStore vectorStore, IVectorEmbedder embedder, IAuditLogger audit) : base(vectorStore, embedder, audit) { }
    public override string Name => "performance-agent";
    protected override string BuildPrompt(AgentRequest request, IReadOnlyList<RetrievalResult> hits)
        => $"Provide performance KPIs, ingestion latency trends, and agent quality diagnostics. Query: {request.Query}. Hits: {hits.Count}";
}
