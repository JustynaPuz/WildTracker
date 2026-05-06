import 'leaflet/dist/leaflet.css'
import { useCallback, useEffect, useRef, useState } from 'react'
import { MapContainer, TileLayer, CircleMarker, Popup } from 'react-leaflet'
import { reportsApi } from '../api/reports'
import { animalsApi } from '../api/animals'
import type { AnimalDto, ReportStatus, SightingReportDto } from '../types/api'

const STATUS_COLOR: Record<ReportStatus, string> = {
  Pending:  '#d97706',
  Verified: '#2d6a4f',
  Rejected: '#c53030',
  Resolved: '#6b7280',
}

const STATUS_FILL: Record<ReportStatus, string> = {
  Pending:  '#fbbf24',
  Verified: '#4ade80',
  Rejected: '#f87171',
  Resolved: '#9ca3af',
}

const STATUSES: ReportStatus[] = ['Pending', 'Verified', 'Rejected', 'Resolved']

// Poland center
const DEFAULT_CENTER: [number, number] = [52.0, 19.0]
const DEFAULT_ZOOM = 6

export default function MapPage() {
  const [reports, setReports]         = useState<SightingReportDto[]>([])
  const [animals, setAnimals]         = useState<AnimalDto[]>([])
  const [filterAnimal, setFilterAnimal] = useState('')
  const [filterStatus, setFilterStatus] = useState<ReportStatus | ''>('')
  const [loading, setLoading]         = useState(true)
  const [error, setError]             = useState<string | null>(null)
  const [totalFetched, setTotalFetched] = useState(0)

  // keep a stable animal lookup map
  const animalMap = useRef<Map<string, AnimalDto>>(new Map())
  useEffect(() => {
    animalMap.current = new Map(animals.map(a => [a.id, a]))
  }, [animals])

  const loadData = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const [animalList, result] = await Promise.all([
        animalsApi.getAll(),
        reportsApi.search({
          animalId:  filterAnimal  || undefined,
          status:    filterStatus  || undefined,
          pageSize:  200,
          page:      1,
        }),
      ])
      setAnimals(animalList)
      setReports(result.items)
      setTotalFetched(result.totalCount)
    } catch {
      setError('Failed to load map data.')
    } finally {
      setLoading(false)
    }
  }, [filterAnimal, filterStatus])

  useEffect(() => { loadData() }, [loadData])

  const animalName = (id: string) =>
    animalMap.current.get(id)?.name ?? id.slice(0, 8) + '…'

  const animalSpecies = (id: string) =>
    animalMap.current.get(id)?.species ?? '—'

  return (
    <div style={{ display: 'flex', flexDirection: 'column', height: 'calc(100vh - 56px - 3rem)' }}>
      {/* ── Toolbar ─────────────────────────────────────────────────────── */}
      <div style={{ display: 'flex', gap: '1rem', alignItems: 'center', marginBottom: '0.75rem', flexWrap: 'wrap' }}>
        <h1 style={{ color: '#2d6a4f', margin: 0, fontSize: '1.5rem' }}>Sightings Map</h1>

        <label style={{ marginLeft: 'auto' }}>
          Animal&nbsp;
          <select value={filterAnimal} onChange={e => { setFilterAnimal(e.target.value) }}
            style={{ padding: '5px', borderRadius: '4px', border: '1px solid #ccc' }}>
            <option value="">All animals</option>
            {animals.map(a => <option key={a.id} value={a.id}>{a.name}</option>)}
          </select>
        </label>

        <label>
          Status&nbsp;
          <select value={filterStatus} onChange={e => setFilterStatus(e.target.value as ReportStatus | '')}
            style={{ padding: '5px', borderRadius: '4px', border: '1px solid #ccc' }}>
            <option value="">All statuses</option>
            {STATUSES.map(s => <option key={s}>{s}</option>)}
          </select>
        </label>

        <span style={{ fontSize: '0.85rem', color: '#555' }}>
          {loading ? 'Loading…' : `${reports.length}${totalFetched > 200 ? '+' : ''} sighting${reports.length !== 1 ? 's' : ''}`}
        </span>
      </div>

      {error && <p style={{ color: '#c53030', margin: '0 0 0.5rem' }}>{error}</p>}

      {/* ── Legend ──────────────────────────────────────────────────────── */}
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '0.5rem', flexWrap: 'wrap' }}>
        {STATUSES.map(s => (
          <span key={s} style={{ display: 'flex', alignItems: 'center', gap: '4px', fontSize: '0.8rem' }}>
            <span style={{ display: 'inline-block', width: 12, height: 12, borderRadius: '50%', background: STATUS_FILL[s], border: `2px solid ${STATUS_COLOR[s]}` }} />
            {s}
          </span>
        ))}
      </div>

      {/* ── Map ─────────────────────────────────────────────────────────── */}
      <div style={{ flex: 1, borderRadius: '8px', overflow: 'hidden', border: '1px solid #d1d5db' }}>
        <MapContainer
          center={DEFAULT_CENTER}
          zoom={DEFAULT_ZOOM}
          style={{ height: '100%', width: '100%' }}
        >
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />

          {reports.map(r => (
            <CircleMarker
              key={r.id}
              center={[r.location.latitude, r.location.longitude]}
              radius={8}
              pathOptions={{
                color:       STATUS_COLOR[r.status],
                fillColor:   STATUS_FILL[r.status],
                fillOpacity: 0.85,
                weight:      2,
              }}
            >
              <Popup>
                <ReportPopup report={r} name={animalName(r.animalId)} species={animalSpecies(r.animalId)} />
              </Popup>
            </CircleMarker>
          ))}
        </MapContainer>
      </div>
    </div>
  )
}

// ── Popup content ─────────────────────────────────────────────────────────────

function ReportPopup({ report: r, name, species }: {
  report: SightingReportDto
  name: string
  species: string
}) {
  const statusStyle = { color: STATUS_COLOR[r.status], fontWeight: 700 }

  return (
    <div style={{ fontSize: '0.85rem', minWidth: '180px' }}>
      <div style={{ fontWeight: 700, fontSize: '0.95rem', marginBottom: '4px' }}>{name}</div>
      <div style={{ color: '#555', marginBottom: '6px' }}>{species}</div>
      <table style={{ borderCollapse: 'collapse', width: '100%' }}>
        <tbody>
          <tr>
            <td style={{ color: '#888', paddingRight: '8px' }}>Status</td>
            <td style={statusStyle}>{r.status}</td>
          </tr>
          <tr>
            <td style={{ color: '#888', paddingRight: '8px' }}>Type</td>
            <td>{r.reportType}</td>
          </tr>
          <tr>
            <td style={{ color: '#888', paddingRight: '8px' }}>Source</td>
            <td>{r.source}</td>
          </tr>
          <tr>
            <td style={{ color: '#888', paddingRight: '8px' }}>Observed</td>
            <td>{new Date(r.observedAtUtc).toLocaleDateString()}</td>
          </tr>
          <tr>
            <td style={{ color: '#888', paddingRight: '8px' }}>Coords</td>
            <td style={{ fontFamily: 'monospace' }}>
              {r.location.latitude.toFixed(4)}, {r.location.longitude.toFixed(4)}
            </td>
          </tr>
          {r.location.region && (
            <tr>
              <td style={{ color: '#888', paddingRight: '8px' }}>Region</td>
              <td>{r.location.region}</td>
            </tr>
          )}
          {r.description && (
            <tr>
              <td colSpan={2} style={{ color: '#444', paddingTop: '6px', fontStyle: 'italic' }}>
                {r.description}
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  )
}
