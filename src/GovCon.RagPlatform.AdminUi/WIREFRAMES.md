# GovCon SaaS UX Wireframes (React Dashboard)

## 1) Executive Overview
- KPI cards: ingestion throughput, webhook success %, reconciliation lag, proposal win probability trend.
- Heat map: source quality by connector (SharePoint, SQL, Excel, Custom).
- Agent activity: Accounts / Contracts / Ops routing split.

## 2) Ingestion Control Plane
- Source onboarding wizard with auth + schema mapping.
- Webhook management (subscriptions, failure retries, dead-letter topic replay).
- Chunking policy editor (token size, overlap, PII redaction profile).

## 3) Knowledge Graph + Vector Explorer
- LiteGraph explorer (entity relationships for contracts, contractors, agencies).
- SharpCoreDB vector index browser (chunk inspection + metadata filters).
- Lineage panel for FedRAMP audit trail (who ingested what, when, and why).

## 4) Agent Ops Center
- Multi-agent router stream (Accounts / Contracts / Ops + specialized agents).
- Past Performance review workspace with cross-source evidence panel.
- RFP matcher + automatic proposal generation workspace.
- Competitor analysis board with win-probability scenarios.

## 5) Audit + Compliance
- Immutable event stream timeline.
- Exportable audit packets for ATO/security review.
- Policy dashboard: retention, encryption, access-control violations.

## 6) Monitoring Dashboard
- Queue depth and consumer lag (Wolverine MQTT topics).
- End-to-end latency percentile charts.
- Agent quality eval panel (groundedness, hallucination, citation coverage).
