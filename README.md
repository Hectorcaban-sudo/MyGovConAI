# MyGovConAI Enterprise RAG Platform (C# Agent Framework + LiteGraph + SharpCoreDB)

This repository now contains a full **enterprise RAG ingestion and agentic architecture** for GovCon workloads using:

- **Microsoft C# AI Agent Framework** (not Semantic Kernel)
- **OpenAI LLM provider wiring**
- **LiteGraph** for graph persistence
- **SharpCoreDB.VectorSearch** extension for vector index storage
- **WolverineFX MQTT transport** for distributed ingestion messaging (no Azure Service Bus)
- **SharePoint webhook-first ingestion** + reconciliation fallback job
- **Multi-agent routing** (Accounts / Contracts / Ops)
- **Past Performance + Proposal + Competitor analysis agents**
- **Admin API + React dashboard wireframes + monitoring + full audit trail**

## Solution layout

- `src/GovCon.RagPlatform.Api` — Admin API + orchestration + webhook endpoints
- `src/GovCon.RagPlatform.Worker` — distributed ingestion workers and reconciliation scheduler
- `src/GovCon.RagPlatform.Core` — shared ingestion, storage, agent, and audit abstractions
- `src/GovCon.RagPlatform.AdminUi` — React wireframe for enterprise SaaS admin UX

## Key implementation notes

1. **No Semantic Kernel** usage.
2. Agent workflows are modeled using Microsoft Agent Framework style agent + workflow abstractions.
3. `SharpCoreDb.VectorSearchExtensions` is extended with chunk-level upsert and source metadata support.
4. All sources (SharePoint, SQL, Excel, custom) feed a common ingestion pipeline so every source is queryable by the Past Performance agent.
5. FedRAMP-style audit events are emitted for ingest, retrieval, routing, proposal generation, and analyst actions.
