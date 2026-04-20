import React from "react";
import "./styles.css";

const Card = ({ title, value, sub }: { title: string; value: string; sub: string }) => (
  <div className="card">
    <h3>{title}</h3>
    <p className="value">{value}</p>
    <p className="sub">{sub}</p>
  </div>
);

export default function App() {
  return (
    <div className="shell">
      <header>
        <h1>GovCon Enterprise RAG Admin</h1>
        <p>Ingestion + Agents + Compliance + Proposal Intelligence</p>
      </header>

      <section className="grid">
        <Card title="Ingestion Throughput" value="2.1M chunks/day" sub="SharePoint + SQL + Excel + Custom" />
        <Card title="Webhook Reliability" value="99.2%" sub="Auto-reconciliation enabled" />
        <Card title="Queue Health" value="Healthy" sub="Wolverine MQTT transport" />
        <Card title="Proposal Win Probability" value="64%" sub="Competitor analysis model" />
      </section>

      <section className="panel">
        <h2>Agent Router</h2>
        <ul>
          <li>Accounts Agent</li>
          <li>Contracts Agent</li>
          <li>Ops Agent</li>
          <li>Past Performance Agent</li>
          <li>RFP / Proposal Matching Agent</li>
          <li>Proposal Generation Agent</li>
          <li>Competitor Analysis Agent</li>
          <li>Performance Agent</li>
        </ul>
      </section>

      <section className="panel">
        <h2>FedRAMP Audit Trail</h2>
        <p>Every ingestion, retrieval, route, generation, and export action is logged with correlation IDs and immutable timestamps.</p>
      </section>
    </div>
  );
}
