import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { statsApi } from '../api/stats'
import { reportsApi } from '../api/reports'
import { animalsApi } from '../api/animals'
import type { AnimalDto, ReportStatus, SightingReportDto, StatsSummaryDto } from '../types/api'

const STATUS_COLOR: Record<ReportStatus, string> = {
  Pending:  '#d97706',
  Verified: '#2d6a4f',
  Rejected: '#c53030',
  Resolved: '#6b7280',
}

export default function DashboardPage() {
  const [summary, setSummary]     = useState<StatsSummaryDto | null>(null)
  const [recent, setRecent]       = useState<SightingReportDto[]>([])
  const [animals, setAnimals]     = useState<AnimalDto[]>([])
  const [loading, setLoading]     = useState(true)
  const [error, setError]         = useState<string | null>(null)

  useEffect(() => {
    Promise.all([
      statsApi.getSummary(),
      reportsApi.search({ page: 1, pageSize: 5 }),
      animalsApi.getAll(),
    ])
      .then(([s, r, a]) => {
        setSummary(s)
        setRecent(r.items)
        setAnimals(a)
      })
      .catch(() => setError('Failed to load dashboard data.'))
      .finally(() => setLoading(false))
  }, [])

  const animalName = (id: string) => animals.find(a => a.id === id)?.name ?? id.slice(0, 8) + '…'

  if (loading) return <p>Loading dashboard…</p>
  if (error)   return <p style={{ color: '#c53030' }}>{error}</p>

  return (
    <div>
      <h1 style={{ color: '#2d6a4f', marginBottom: '1.5rem' }}>Dashboard</h1>

      {/* ── Metric cards ──────────────────────────────────────────────── */}
      {summary && (
        <section style={{ marginBottom: '2rem' }}>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '1rem' }}>
            <MetricCard
              label="Tracked Animals"
              value={summary.totalAnimals}
              color="#2d6a4f"
              icon="🐾"
              href="/animals"
            />
            <MetricCard
              label="Pending Reports"
              value={summary.pendingReports}
              color="#d97706"
              icon="⏳"
              href="/reports"
              note="need review"
            />
            <MetricCard
              label="Verified Reports"
              value={summary.verifiedReports}
              color="#059669"
              icon="✓"
              href="/reports"
            />
            <MetricCard
              label="Total Reports"
              value={summary.totalReports}
              color="#1d4ed8"
              icon="📋"
              href="/reports"
            />
            <MetricCard
              label="Observation Notes"
              value={summary.totalNotes}
              color="#7c3aed"
              icon="📝"
              href="/reports"
            />
            <MetricCard
              label="Rejected Reports"
              value={summary.rejectedReports}
              color="#c53030"
              icon="✕"
              href="/reports"
            />
          </div>
        </section>
      )}

      <div style={{ display: 'grid', gridTemplateColumns: '1fr auto', gap: '2rem', alignItems: 'start' }}>
        {/* ── Recent reports ──────────────────────────────────────────── */}
        <section>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'baseline', marginBottom: '0.75rem' }}>
            <h2 style={{ margin: 0, fontSize: '1rem', color: '#374151', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
              Recent Sightings
            </h2>
            <Link to="/reports" style={{ fontSize: '0.85rem', color: '#2d6a4f' }}>View all →</Link>
          </div>

          {recent.length === 0 ? (
            <p style={{ color: '#666' }}>No reports yet.</p>
          ) : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
              {recent.map(r => (
                <div key={r.id} style={{
                  display: 'grid',
                  gridTemplateColumns: '1fr auto',
                  gap: '0.75rem',
                  padding: '0.75rem 1rem',
                  background: '#fff',
                  border: '1px solid #e5e7eb',
                  borderRadius: '8px',
                  alignItems: 'center',
                }}>
                  <div>
                    <div style={{ fontWeight: 600, marginBottom: '2px' }}>{animalName(r.animalId)}</div>
                    <div style={{ fontSize: '0.8rem', color: '#6b7280' }}>
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
                </div>
              ))}
            </div>
          )}
        </section>

        {/* ── Quick links ──────────────────────────────────────────────── */}
        <section style={{ minWidth: '200px' }}>
          <h2 style={{ margin: '0 0 0.75rem', fontSize: '1rem', color: '#374151', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
            Quick Links
          </h2>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            <QuickLink to="/animals" label="Animals"    description="Manage tracked animals" />
            <QuickLink to="/reports" label="Reports"    description="Review sightings" />
            <QuickLink to="/map"     label="Map"        description="View sightings on map" />
            <QuickLink to="/stats"   label="Statistics" description="Charts & counts" />
          </div>
        </section>
      </div>
    </div>
  )
}

// ── Metric card ───────────────────────────────────────────────────────────────

interface MetricCardProps {
  label: string
  value: number
  color: string
  icon: string
  href: string
  note?: string
}

function MetricCard({ label, value, color, icon, href, note }: MetricCardProps) {
  return (
    <Link to={href} style={{ textDecoration: 'none' }}>
      <div style={{
        background: '#fff',
        border: `1px solid #e5e7eb`,
        borderLeft: `4px solid ${color}`,
        borderRadius: '8px',
        padding: '1rem 1.25rem',
        boxShadow: '0 1px 3px rgba(0,0,0,0.06)',
        transition: 'box-shadow 0.15s',
        cursor: 'pointer',
      }}
        onMouseEnter={e => (e.currentTarget.style.boxShadow = '0 4px 12px rgba(0,0,0,0.12)')}
        onMouseLeave={e => (e.currentTarget.style.boxShadow = '0 1px 3px rgba(0,0,0,0.06)')}
      >
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <div>
            <div style={{ fontSize: '2rem', fontWeight: 700, color, lineHeight: 1 }}>{value}</div>
            <div style={{ fontSize: '0.8rem', color: '#6b7280', marginTop: '4px' }}>{label}</div>
            {note && <div style={{ fontSize: '0.75rem', color, marginTop: '2px' }}>{note}</div>}
          </div>
          <span style={{ fontSize: '1.5rem', opacity: 0.6 }}>{icon}</span>
        </div>
      </div>
    </Link>
  )
}

// ── Quick link card ───────────────────────────────────────────────────────────

function QuickLink({ to, label, description }: { to: string; label: string; description: string }) {
  return (
    <Link to={to} style={{ textDecoration: 'none' }}>
      <div style={{
        padding: '0.6rem 0.9rem',
        background: '#fff',
        border: '1px solid #e5e7eb',
        borderRadius: '6px',
        boxShadow: '0 1px 2px rgba(0,0,0,0.05)',
        transition: 'background 0.15s',
      }}
        onMouseEnter={e => (e.currentTarget.style.background = '#f0f7f0')}
        onMouseLeave={e => (e.currentTarget.style.background = '#fff')}
      >
        <div style={{ fontWeight: 600, color: '#2d6a4f', fontSize: '0.9rem' }}>{label}</div>
        <div style={{ fontSize: '0.75rem', color: '#9ca3af' }}>{description}</div>
      </div>
    </Link>
  )
}
