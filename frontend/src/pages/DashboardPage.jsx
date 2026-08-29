import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { statsApi } from '../api/stats';
import { reportsApi } from '../api/reports';
import { animalsApi } from '../api/animals';
import { STATUS_COLOR } from '../constants';
export default function DashboardPage() {
    const [summary, setSummary] = useState(null);
    const [recent, setRecent] = useState([]);
    const [animals, setAnimals] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    useEffect(() => {
        Promise.all([
            statsApi.getSummary(),
            reportsApi.search({ page: 1, pageSize: 5 }),
            animalsApi.getAll(),
        ])
            .then(([s, r, a]) => {
            setSummary(s);
            setRecent(r.items);
            setAnimals(a);
        })
            .catch(() => setError('Failed to load dashboard data.'))
            .finally(() => setLoading(false));
    }, []);
    const animalName = (id) => animals.find(a => a.id === id)?.name ?? id.slice(0, 8) + '…';
    if (loading)
        return <p>Loading dashboard…</p>;
    if (error)
        return <p style={{ color: '#f87171' }}>{error}</p>;
    return (<div>
      <h1 style={{ color: '#34d399', marginBottom: '1.5rem' }}>Dashboard</h1>

      {/* ── Metric cards ──────────────────────────────────────────────── */}
      {summary && (<section style={{ marginBottom: '2rem' }}>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '1rem' }}>
            <MetricCard label="Tracked Animals" value={summary.totalAnimals} color="#34d399" icon="🐾"/>
            <MetricCard label="Pending Reports" value={summary.pendingReports} color="#d97706" icon="⏳" note="need review"/>
            <MetricCard label="Verified Reports" value={summary.verifiedReports} color="#059669" icon="✓"/>
            <MetricCard label="Total Reports" value={summary.totalReports} color="#1d4ed8" icon="📋"/>
            <MetricCard label="Observation Notes" value={summary.totalNotes} color="#7c3aed" icon="📝"/>
            <MetricCard label="Rejected Reports" value={summary.rejectedReports} color="#c53030" icon="✕"/>
          </div>
        </section>)}

      {/* ── Recent reports ──────────────────────────────────────────── */}
      <section>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'baseline', marginBottom: '0.75rem' }}>
          <h2 style={{ margin: 0, fontSize: '1rem', color: '#d4d4d4', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
            Recent Sightings
          </h2>
          <Link to="/reports" style={{ fontSize: '0.85rem', color: '#34d399' }}>View all →</Link>
        </div>

        {recent.length === 0 ? (<p style={{ color: '#8f8f8f' }}>No reports yet.</p>) : (<div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            {recent.map(r => (<div key={r.id} style={{
                  display: 'grid',
                  gridTemplateColumns: '1fr auto',
                  gap: '0.75rem',
                  padding: '0.75rem 1rem',
                  background: '#1e1e1e',
                  border: '1px solid #2a2a2a',
                  borderRadius: '8px',
                  alignItems: 'center',
              }}>
                <div>
                  <div style={{ fontWeight: 600, marginBottom: '2px', color: '#e8e8e8' }}>{animalName(r.animalId)}</div>
                  <div style={{ fontSize: '0.8rem', color: '#9a9a9a' }}>
                    {r.reportType} · {r.source} ·{' '}
                    {r.location.region ?? `${r.location.latitude.toFixed(3)}, ${r.location.longitude.toFixed(3)}`} ·{' '}
                    {new Date(r.observedAtUtc).toLocaleDateString()}
                  </div>
                </div>
                <span style={{
                  fontSize: '0.75rem',
                  fontWeight: 700,
                  color: STATUS_COLOR[r.status],
                  padding: '3px 8px',
                  borderRadius: '12px',
                  border: `1px solid ${STATUS_COLOR[r.status]}`,
                  whiteSpace: 'nowrap',
              }}>
                  {r.status}
                </span>
              </div>))}
          </div>)}
      </section>
    </div>);
}
function MetricCard({ label, value, color, icon, href, note }) {
    const card = (<div style={{
            background: '#1e1e1e',
            border: `1px solid #2a2a2a`,
            borderLeft: `4px solid ${color}`,
            borderRadius: '8px',
            padding: '1rem 1.25rem',
            boxShadow: '0 1px 3px rgba(0,0,0,0.3)',
            transition: 'box-shadow 0.15s',
            cursor: href ? 'pointer' : 'default',
        }} onMouseEnter={e => href && (e.currentTarget.style.boxShadow = '0 4px 12px rgba(0,0,0,0.5)')} onMouseLeave={e => href && (e.currentTarget.style.boxShadow = '0 1px 3px rgba(0,0,0,0.3)')}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <div>
          <div style={{ fontSize: '2rem', fontWeight: 700, color, lineHeight: 1 }}>{value}</div>
          <div style={{ fontSize: '0.8rem', color: '#9a9a9a', marginTop: '4px' }}>{label}</div>
          {note && <div style={{ fontSize: '0.75rem', color, marginTop: '2px' }}>{note}</div>}
        </div>
        <span style={{ fontSize: '1.5rem', opacity: 0.6 }}>{icon}</span>
      </div>
    </div>);
    return href ? <Link to={href} style={{ textDecoration: 'none' }}>{card}</Link> : card;
}
