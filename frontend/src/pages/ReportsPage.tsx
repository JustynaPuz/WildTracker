import { useCallback, useEffect, useState } from 'react'
import { reportsApi } from '../api/reports'
import { animalsApi } from '../api/animals'
import { notesApi } from '../api/notes'
import type {
  SightingReportDto,
  AnimalDto,
  ObservationNoteDto,
  CreateSightingReportRequest,
  ReportStatus,
  ReportType,
  SightingSource,
} from '../types/api'

const REPORT_TYPES: ReportType[] = ['Sighting', 'Injury', 'Death', 'DangerousBehavior', 'Other']
const SOURCES: SightingSource[] = ['Manual', 'CameraTrap', 'Drone', 'Sensor', 'Imported']
const STATUSES: ReportStatus[] = ['Pending', 'Verified', 'Rejected', 'Resolved']

const statusColor: Record<ReportStatus, string> = {
  Pending:  '#b7791f',
  Verified: '#276749',
  Rejected: '#c53030',
  Resolved: '#2b6cb0',
}

const btn = (bg: string): React.CSSProperties => ({
  background: bg, color: '#fff', border: 'none',
  padding: '3px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem',
})

const emptyForm = (): CreateSightingReportRequest => ({
  animalId: '',
  observedAtUtc: new Date().toISOString().slice(0, 16),
  reportType: 'Sighting',
  source: 'Manual',
  latitude: 0,
  longitude: 0,
  region: '',
  forestDistrict: '',
  description: '',
})

export default function ReportsPage() {
  const [animals, setAnimals]   = useState<AnimalDto[]>([])
  const [reports, setReports]   = useState<SightingReportDto[]>([])
  const [total, setTotal]       = useState(0)
  const [page, setPage]         = useState(1)
  const PAGE_SIZE               = 10

  const [filterAnimal, setFilterAnimal]   = useState('')
  const [filterStatus, setFilterStatus]   = useState<ReportStatus | ''>('')
  const [loading, setLoading]             = useState(true)
  const [error, setError]                 = useState<string | null>(null)

  const [showForm, setShowForm] = useState(false)
  const [form, setForm]         = useState<CreateSightingReportRequest>(emptyForm())

  const [expandedId, setExpandedId] = useState<string | null>(null)
  const [notes, setNotes]           = useState<Record<string, ObservationNoteDto[]>>({})
  const [noteContent, setNoteContent] = useState('')

  // per-note edit state: noteId → draft content (undefined = not editing)
  const [editingNote, setEditingNote] = useState<Record<string, string>>({})

  useEffect(() => { animalsApi.getAll().then(setAnimals).catch(() => {}) }, [])

  const loadReports = useCallback(() => {
    setLoading(true)
    reportsApi.search({
      animalId: filterAnimal || undefined,
      status:   filterStatus || undefined,
      page,
      pageSize: PAGE_SIZE,
    })
      .then((r) => { setReports(r.items); setTotal(r.totalCount) })
      .catch(() => setError('Failed to load reports.'))
      .finally(() => setLoading(false))
  }, [filterAnimal, filterStatus, page])

  useEffect(() => { loadReports() }, [loadReports])

  // ── report actions ────────────────────────────────────────────────────────

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      await reportsApi.create({
        ...form,
        observedAtUtc: new Date(form.observedAtUtc).toISOString(),
        region:         form.region         || undefined,
        forestDistrict: form.forestDistrict || undefined,
        description:    form.description    || undefined,
      })
      setShowForm(false)
      setForm(emptyForm())
      loadReports()
    } catch { setError('Failed to create report.') }
  }

  const handleApprove = async (id: string) => {
    try { await reportsApi.approve(id); loadReports() }
    catch { setError('Failed to approve.') }
  }

  const handleReject = async (id: string) => {
    try { await reportsApi.reject(id); loadReports() }
    catch { setError('Failed to reject.') }
  }

  const handleResolve = async (id: string) => {
    try { await reportsApi.resolve(id); loadReports() }
    catch { setError('Failed to resolve.') }
  }

  const handleDeleteReport = async (id: string) => {
    if (!confirm('Delete this report?')) return
    try { await reportsApi.delete(id); loadReports() }
    catch { setError('Failed to delete.') }
  }

  // ── note actions ──────────────────────────────────────────────────────────

  const reloadNotes = async (reportId: string) => {
    const data = await notesApi.getByReport(reportId).catch(() => [])
    setNotes((prev) => ({ ...prev, [reportId]: data }))
  }

  const toggleNotes = async (id: string) => {
    if (expandedId === id) { setExpandedId(null); return }
    setExpandedId(id)
    if (!notes[id]) await reloadNotes(id)
  }

  const handleAddNote = async (reportId: string) => {
    if (!noteContent.trim()) return
    try {
      await notesApi.create(reportId, { content: noteContent })
      setNoteContent('')
      await reloadNotes(reportId)
    } catch { setError('Failed to add note.') }
  }

  const handleEditNote = (noteId: string, current: string) =>
    setEditingNote((prev) => ({ ...prev, [noteId]: current }))

  const handleCancelEdit = (noteId: string) =>
    setEditingNote((prev) => { const next = { ...prev }; delete next[noteId]; return next })

  const handleSaveNote = async (reportId: string, noteId: string) => {
    const content = editingNote[noteId]?.trim()
    if (!content) return
    try {
      await notesApi.update(reportId, noteId, { content })
      handleCancelEdit(noteId)
      await reloadNotes(reportId)
    } catch { setError('Failed to update note.') }
  }

  const handleDeleteNote = async (reportId: string, noteId: string) => {
    if (!confirm('Delete this note?')) return
    try {
      await notesApi.delete(reportId, noteId)
      await reloadNotes(reportId)
    } catch { setError('Failed to delete note.') }
  }

  // ── helpers ───────────────────────────────────────────────────────────────

  const totalPages = Math.ceil(total / PAGE_SIZE)
  const animalName = (id: string) => animals.find((a) => a.id === id)?.name ?? id.slice(0, 8)

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
        <h1 style={{ color: '#2d6a4f', margin: 0 }}>Sighting Reports</h1>
        <button onClick={() => setShowForm(!showForm)}
          style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>
          + New Report
        </button>
      </div>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Filters */}
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', flexWrap: 'wrap' }}>
        <label>
          Animal&nbsp;
          <select value={filterAnimal} onChange={(e) => { setFilterAnimal(e.target.value); setPage(1) }}
            style={{ padding: '6px', borderRadius: '4px', border: '1px solid #ccc' }}>
            <option value="">All animals</option>
            {animals.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
          </select>
        </label>
        <label>
          Status&nbsp;
          <select value={filterStatus} onChange={(e) => { setFilterStatus(e.target.value as ReportStatus | ''); setPage(1) }}
            style={{ padding: '6px', borderRadius: '4px', border: '1px solid #ccc' }}>
            <option value="">All statuses</option>
            {STATUSES.map((s) => <option key={s}>{s}</option>)}
          </select>
        </label>
      </div>

      {/* Create form */}
      {showForm && (
        <form onSubmit={handleCreate} style={{ background: '#f0f7f0', padding: '1rem', borderRadius: '8px', marginBottom: '1.5rem', display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '0.75rem' }}>
          <label>
            Animal *
            <select value={form.animalId} onChange={(e) => setForm((f) => ({ ...f, animalId: e.target.value }))} required
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }}>
              <option value="">Select…</option>
              {animals.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
            </select>
          </label>
          <label>
            Observed At *
            <input type="datetime-local" value={form.observedAtUtc}
              onChange={(e) => setForm((f) => ({ ...f, observedAtUtc: e.target.value }))} required
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }} />
          </label>
          <label>
            Report Type
            <select value={form.reportType} onChange={(e) => setForm((f) => ({ ...f, reportType: e.target.value as ReportType }))}
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }}>
              {REPORT_TYPES.map((t) => <option key={t}>{t}</option>)}
            </select>
          </label>
          <label>
            Source
            <select value={form.source} onChange={(e) => setForm((f) => ({ ...f, source: e.target.value as SightingSource }))}
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }}>
              {SOURCES.map((s) => <option key={s}>{s}</option>)}
            </select>
          </label>
          <label>
            Latitude *
            <input type="number" step="any" value={form.latitude}
              onChange={(e) => setForm((f) => ({ ...f, latitude: parseFloat(e.target.value) || 0 }))} required
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }} />
          </label>
          <label>
            Longitude *
            <input type="number" step="any" value={form.longitude}
              onChange={(e) => setForm((f) => ({ ...f, longitude: parseFloat(e.target.value) || 0 }))} required
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }} />
          </label>
          <label>
            Region
            <input value={form.region ?? ''} onChange={(e) => setForm((f) => ({ ...f, region: e.target.value }))}
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }} />
          </label>
          <label>
            Forest District
            <input value={form.forestDistrict ?? ''} onChange={(e) => setForm((f) => ({ ...f, forestDistrict: e.target.value }))}
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }} />
          </label>
          <label style={{ gridColumn: '1 / -1' }}>
            Description
            <textarea value={form.description ?? ''} onChange={(e) => setForm((f) => ({ ...f, description: e.target.value }))}
              rows={2} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }} />
          </label>
          <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
            <button type="submit" style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>Save</button>
            <button type="button" onClick={() => setShowForm(false)} style={{ padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>Cancel</button>
          </div>
        </form>
      )}

      {/* Reports table */}
      {loading ? <p>Loading…</p> : (
        <>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead>
              <tr style={{ background: '#d8f3dc', textAlign: 'left' }}>
                {['Animal', 'Type', 'Source', 'Location', 'Observed', 'Status', 'Actions'].map((h) => (
                  <th key={h} style={{ padding: '8px 12px', borderBottom: '2px solid #2d6a4f' }}>{h}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {reports.map((r) => (
                <>
                  <tr key={r.id} style={{ borderBottom: expandedId === r.id ? 'none' : '1px solid #eee' }}>
                    <td style={{ padding: '8px 12px' }}>{animalName(r.animalId)}</td>
                    <td style={{ padding: '8px 12px' }}>{r.reportType}</td>
                    <td style={{ padding: '8px 12px' }}>{r.source}</td>
                    <td style={{ padding: '8px 12px' }}>
                      {r.location.latitude.toFixed(4)}, {r.location.longitude.toFixed(4)}
                      {r.location.region && <span style={{ color: '#666', fontSize: '0.8rem' }}> {r.location.region}</span>}
                    </td>
                    <td style={{ padding: '8px 12px' }}>{new Date(r.observedAtUtc).toLocaleDateString()}</td>
                    <td style={{ padding: '8px 12px' }}>
                      <span style={{ color: statusColor[r.status], fontWeight: 600 }}>{r.status}</span>
                    </td>
                    <td style={{ padding: '8px 12px' }}>
                      <div style={{ display: 'flex', gap: '4px', flexWrap: 'wrap' }}>
                        {r.status === 'Pending' && (
                          <>
                            <button onClick={() => handleApprove(r.id)} style={btn('#276749')}>Approve</button>
                            <button onClick={() => handleReject(r.id)}  style={btn('#c53030')}>Reject</button>
                          </>
                        )}
                        {r.status === 'Verified' && (
                          <button onClick={() => handleResolve(r.id)} style={btn('#6b46c1')}>Resolve</button>
                        )}
                        <button onClick={() => toggleNotes(r.id)} style={btn('#2b6cb0')}>
                          {expandedId === r.id ? 'Hide Notes' : 'Notes'}
                        </button>
                        <button onClick={() => handleDeleteReport(r.id)} style={btn('#718096')}>Delete</button>
                      </div>
                    </td>
                  </tr>

                  {expandedId === r.id && (
                    <tr key={`${r.id}-notes`}>
                      <td colSpan={7} style={{ padding: '0 12px 12px 12px', background: '#f7fafc', borderBottom: '1px solid #eee' }}>
                        <div style={{ paddingTop: '8px' }}>
                          {(notes[r.id] ?? []).length === 0
                            ? <p style={{ color: '#666', margin: '0 0 8px 0' }}>No notes yet.</p>
                            : (notes[r.id] ?? []).map((n) => (
                                <div key={n.id} style={{ marginBottom: '8px', padding: '8px', background: '#fff', borderRadius: '4px', border: '1px solid #e2e8f0' }}>
                                  {editingNote[n.id] !== undefined ? (
                                    /* ── edit mode ── */
                                    <div>
                                      <textarea
                                        value={editingNote[n.id]}
                                        onChange={(e) => setEditingNote((prev) => ({ ...prev, [n.id]: e.target.value }))}
                                        rows={3}
                                        style={{ width: '100%', padding: '6px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box' }}
                                      />
                                      <div style={{ display: 'flex', gap: '6px', marginTop: '4px' }}>
                                        <button onClick={() => handleSaveNote(r.id, n.id)} style={btn('#276749')}>Save</button>
                                        <button onClick={() => handleCancelEdit(n.id)}
                                          style={{ border: '1px solid #ccc', background: '#fff', padding: '3px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                                          Cancel
                                        </button>
                                      </div>
                                    </div>
                                  ) : (
                                    /* ── read mode ── */
                                    <div>
                                      <p style={{ margin: '0 0 4px 0' }}>{n.content}</p>
                                      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                        <small style={{ color: '#888' }}>
                                          {new Date(n.createdAtUtc).toLocaleString()}
                                          {n.updatedAtUtc && <span> · edited {new Date(n.updatedAtUtc).toLocaleString()}</span>}
                                        </small>
                                        <div style={{ display: 'flex', gap: '4px' }}>
                                          <button onClick={() => handleEditNote(n.id, n.content)} style={btn('#b7791f')}>Edit</button>
                                          <button onClick={() => handleDeleteNote(r.id, n.id)}    style={btn('#c53030')}>Delete</button>
                                        </div>
                                      </div>
                                    </div>
                                  )}
                                </div>
                              ))
                          }
                          {/* add note */}
                          <div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
                            <input
                              placeholder="Add a note…"
                              value={noteContent}
                              onChange={(e) => setNoteContent(e.target.value)}
                              onKeyDown={(e) => { if (e.key === 'Enter') handleAddNote(r.id) }}
                              style={{ flex: 1, padding: '6px', borderRadius: '4px', border: '1px solid #ccc' }}
                            />
                            <button onClick={() => handleAddNote(r.id)}
                              style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '6px 14px', cursor: 'pointer', borderRadius: '4px' }}>
                              Add
                            </button>
                          </div>
                        </div>
                      </td>
                    </tr>
                  )}
                </>
              ))}
              {reports.length === 0 && (
                <tr><td colSpan={7} style={{ padding: '2rem', textAlign: 'center', color: '#666' }}>No reports found.</td></tr>
              )}
            </tbody>
          </table>

          {totalPages > 1 && (
            <div style={{ display: 'flex', gap: '0.5rem', justifyContent: 'center', marginTop: '1rem' }}>
              <button onClick={() => setPage((p) => Math.max(1, p - 1))} disabled={page === 1}
                style={{ padding: '6px 14px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #ccc' }}>
                Prev
              </button>
              <span style={{ lineHeight: '34px', fontSize: '0.9rem' }}>{page} / {totalPages}</span>
              <button onClick={() => setPage((p) => Math.min(totalPages, p + 1))} disabled={page === totalPages}
                style={{ padding: '6px 14px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #ccc' }}>
                Next
              </button>
            </div>
          )}
        </>
      )}
    </div>
  )
}
