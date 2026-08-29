import { useEffect, useState } from 'react';
import { statsApi } from '../api/stats';
export default function StatsPage() {
    const [summary, setSummary] = useState(null);
    const [species, setSpecies] = useState([]);
    const [monthly, setMonthly] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    useEffect(() => {
        setLoading(true);
        Promise.all([
            statsApi.getSummary(),
            statsApi.getBySpecies(),
            statsApi.getByMonth(12),
        ])
            .then(([s, sp, mo]) => { setSummary(s); setSpecies(sp); setMonthly(mo); })
            .catch(() => setError('Failed to load statistics.'))
            .finally(() => setLoading(false));
    }, []);
    if (loading)
        return <p>Loading statistics…</p>;
    if (error)
        return <p style={{ color: '#f87171' }}>{error}</p>;
    if (!summary)
        return null;
    return (<div>
      <h1 style={{ color: '#34d399', marginBottom: '1.5rem' }}>Statistics</h1>

      {/* ── Summary cards ─────────────────────────────────────────────── */}
      <section style={{ marginBottom: '2rem' }}>
        <h2 style={{ fontSize: '1rem', color: '#b3b3b3', marginBottom: '0.75rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>Overview</h2>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(160px, 1fr))', gap: '1rem' }}>
          <Card label="Animals" value={summary.totalAnimals} color="#34d399"/>
          <Card label="Total Reports" value={summary.totalReports} color="#1d4ed8"/>
          <Card label="Pending" value={summary.pendingReports} color="#d97706"/>
          <Card label="Verified" value={summary.verifiedReports} color="#059669"/>
          <Card label="Rejected" value={summary.rejectedReports} color="#f87171"/>
          <Card label="Resolved" value={summary.resolvedReports} color="#9ca3af"/>
          <Card label="Notes" value={summary.totalNotes} color="#a78bfa"/>
          <Card label="Users" value={summary.totalUsers} color="#22d3ee"/>
        </div>
      </section>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '2rem' }}>
        {/* ── By species ────────────────────────────────────────────────── */}
        <section>
          <h2 style={{ fontSize: '1rem', color: '#b3b3b3', marginBottom: '0.75rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
            Sightings by Species
          </h2>
          {species.length === 0 ? (<p style={{ color: '#8f8f8f' }}>No data yet.</p>) : (<BarChart rows={species.map(s => ({ label: s.species, value: s.count }))} color="#34d399"/>)}
        </section>

        {/* ── By month ──────────────────────────────────────────────────── */}
        <section>
          <h2 style={{ fontSize: '1rem', color: '#b3b3b3', marginBottom: '0.75rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
            Sightings per Month (last 12)
          </h2>
          {monthly.length === 0 ? (<p style={{ color: '#8f8f8f' }}>No data yet.</p>) : (<BarChart rows={monthly.map(m => ({ label: m.label, value: m.count }))} color="#1d4ed8"/>)}
        </section>
      </div>
    </div>);
}
// ── Reusable summary card ─────────────────────────────────────────────────────
function Card({ label, value, color }) {
    return (<div style={{
            background: '#1e1e1e',
            border: `2px solid ${color}`,
            borderRadius: '8px',
            padding: '1rem',
            textAlign: 'center',
            boxShadow: '0 1px 3px rgba(0,0,0,0.3)',
        }}>
      <div style={{ fontSize: '2rem', fontWeight: 700, color }}>{value}</div>
      <div style={{ fontSize: '0.8rem', color: '#b3b3b3', marginTop: '0.25rem' }}>{label}</div>
    </div>);
}
function BarChart({ rows, color }) {
    const max = Math.max(...rows.map(r => r.value), 1);
    return (<div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
      {rows.map(r => (<div key={r.label} style={{ display: 'grid', gridTemplateColumns: '110px 1fr 2.5rem', gap: '0.5rem', alignItems: 'center' }}>
          <span style={{ fontSize: '0.8rem', color: '#a3a3a3', textAlign: 'right', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }} title={r.label}>
            {r.label}
          </span>
          <div style={{ background: '#2a2a2a', borderRadius: '4px', height: '20px', overflow: 'hidden' }}>
            <div style={{
                width: `${(r.value / max) * 100}%`,
                height: '100%',
                background: color,
                borderRadius: '4px',
                transition: 'width 0.4s ease',
            }}/>
          </div>
          <span style={{ fontSize: '0.8rem', color: '#b3b3b3', fontWeight: 600 }}>{r.value}</span>
        </div>))}
    </div>);
}
