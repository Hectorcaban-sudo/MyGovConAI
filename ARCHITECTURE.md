# Enterprise GovCon RAG Platform Architecture

## 1. Technology constraints satisfied

- **Agent framework**: Microsoft C# AI Agent Framework concepts and workflow patterns.
- **LLM**: OpenAI model provider.
- **No Semantic Kernel**.
- **Graph backend**: LiteGraph.
- **Vector storage**: SharpCoreDB (`SharpCoreDb.VectorSearch` extension).
- **Distributed ingestion queue**: WolverineFX with MQTT transport (not Azure Service Bus).
- **Realtime ingestion**: SharePoint webhooks (primary) + scheduled reconciliation fallback.

## 2. Ingestion at 100K+ SharePoint documents

1. SharePoint webhook notifies `/api/webhooks/sharepoint`.
2. Event is normalized to `SourceDocument` and published to Wolverine MQTT topic.
3. Worker handles envelope and executes `IngestionPipeline`:
   - chunking (`SlidingWindowChunker`)
   - embedding (OpenAI)
   - vector upsert (SharpCoreDB)
   - graph upsert (LiteGraph node/edge)
   - audit + metrics
4. Reconciliation job replays missed documents from connectors since checkpoint timestamp.

## 3. Multi-source connectors

All sources map to same schema:

- SharePoint
- SQL/DB
- Excel
- Custom connector (adapter pattern)

This ensures the **Past Performance Review Agent** can use evidence from every source type.

## 4. Multi-agent topology

- Accounts Agent
- Contracts Agent
- Ops Agent
- Past Performance Review Agent
- RFP/Proposal Matching Agent
- Proposal Generation Agent (SOW + technical volume draft)
- Competitor Analysis Agent (bidder set + win probability)
- Performance Agent (platform/agent quality diagnostics)

Routing is handled by `AgentOrchestrator` today (keyword rules), and can later be upgraded to an Agent Framework workflow graph with typed branches.

## 5. Admin APIs and dashboards

Admin/API surfaces:

- `/api/webhooks/sharepoint`
- `/api/agents/query`
- `/api/admin/health`
- `/api/admin/metrics`

React wireframes in `src/GovCon.RagPlatform.AdminUi` provide SaaS UX design foundation for:

- ingestion operations
- graph/vector exploration
- agent operations
- compliance/audit
- performance monitoring

## 6. FedRAMP-style audit traceability

Audit event model captures:

- tenant + correlation IDs
- actor identity
- action/resource
- immutable UTC timestamp
- detail metadata

Every pipeline stage should emit an event and be exportable for compliance evidence.

## 7. Wolverine MQTT baseline snippet

```csharp
builder.Host.UseWolverine(opts =>
{
    opts.UseMqtt(mqtt => mqtt.WithClientOptions(c => c.WithTcpServer("mqtt-broker")));
    opts.ListenToMqttTopic("ingestion/sharepoint");
    opts.PublishAllMessages().ToMqttTopic("ingestion/normalized");
});
```

## 8. Microsoft Agent Framework integration plan

- Define agents using Agent Framework APIs for OpenAI-backed execution.
- Implement workflow graph for multi-agent routing and human approval checkpoints.
- Persist session/workflow state for replay and auditability.

This repo already structures domain components so Agent Framework runtime can be plugged in with minimal refactoring.
