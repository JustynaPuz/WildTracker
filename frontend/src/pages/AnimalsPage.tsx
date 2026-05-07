import { useEffect, useState, useCallback, Fragment } from 'react'
import { animalsApi } from '../api/animals'
import { STATUS_COLOR } from '../constants'
import type { AnimalDto, CreateAnimalRequest, MovementPointDto } from '../types/api'

const SPECIES = ['Unknown','Wolf','Fox','Bear','Deer','Boar','Lynx','Moose','Bird','Other'] as const
const HEALTH  = ['Unknown','Healthy','Injured','Sick','Dead'] as const

export default function AnimalsPage() {
  const [animals, setAnimals]           = useState<AnimalDto[]>([])
  const [loading, setLoading]           = useState(true)
  const [error, setError]               = useState<string | null>(null)
  const [showForm, setShowForm]         = useState(false)
  const [form, setForm]                 = useState<CreateAnimalRequest>({
    identifier: '', name: '', species: 'Unknown', healthStatus: 'Unknown',
  })

  const [editingId, setEditingId]   = useState<string | null>(null)
  const [editForm, setEditForm]     = useState<{ name: string; species: typeof SPECIES[number]; healthStatus: typeof HEALTH[number]; description: string }>({ name: '', species: 'Unknown', healthStatus: 'Unknown', description: '' })

  // movement panel state
  const [movementAnimalId, setMovementAnimalId]     = useState<string | null>(null)
  const [movement, setMovement]                     = useState<MovementPointDto[]>([])
  const [movementLoading, setMovementLoading]       = useState(false)
  const [movementError, setMovementError]           = useState<string | null>(null)

  const load = useCallback(() => {
    setLoading(true)
    animalsApi.getAll()
      .then(setAnimals)
      .catch(() => setError('Failed to load animals.'))
      .finally(() => setLoading(false))
  }, [])

  useEffect(() => { load() }, [load])

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      await animalsApi.create(form)
      setShowForm(false)
      setForm({ identifier: '', name: '', species: 'Unknown', healthStatus: 'Unknown' })
      load()
    } catch {
      setError('Failed to create animal.')
    }
  }

  const handleDelete = async (id: string) => {
    if (!confirm('Delete this animal?')) return
    try {
      await animalsApi.delete(id)
      if (movementAnimalId === id) setMovementAnimalId(null)
      load()
    } catch {
      setError('Failed to delete animal.')
    }
  }

  const openEdit = (a: AnimalDto) => {
    setEditingId(a.id)
    setEditForm({ name: a.name, species: a.species as typeof SPECIES[number], healthStatus: a.healthStatus as typeof HEALTH[number], description: a.description ?? '' })
    setMovementAnimalId(null)
  }

  const handleSaveEdit = async (id: string) => {
    try {
      await animalsApi.update(id, {
        name:         editForm.name,
        species:      editForm.species,
        healthStatus: editForm.healthStatus,
        description:  editForm.description || undefined,
      })
      setEditingId(null)
      load()
    } catch {
      setError('Failed to update animal.')
    }
  }

  const toggleMovement = async (id: string) => {
    if (movementAnimalId === id) {
      setMovementAnimalId(null)
      return
    }
    setEditingId(null)
    setMovementAnimalId(id)
    setMovement([])
    setMovementError(null)
    setMovementLoading(true)
    try {
      const pts = await animalsApi.getMovement(id)
      setMovement(pts)
    } catch {
      setMovementError('Failed to load movement history.')
    } finally {
      setMovementLoading(false)
    }
  }

  const movementAnimal = animals.find(a => a.id === movementAnimalId)

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
        <h1 style={{ color: '#2d6a4f' }}>Animals</h1>
        <button onClick={() => setShowForm(!showForm)}
          style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>
          + Add Animal
        </button>
      </div>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {showForm && (
        <form onSubmit={handleCreate} style={{ background: '#f0f7f0', padding: '1rem', borderRadius: '8px', marginBottom: '1.5rem', display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0.75rem' }}>
          <label>
            Identifier *
            <input value={form.identifier} onChange={e => setForm(f => ({ ...f, identifier: e.target.value }))} required
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box' }} />
          </label>
          <label>
            Name *
            <input value={form.name} onChange={e => setForm(f => ({ ...f, name: e.target.value }))} required
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box' }} />
          </label>
          <label>
            Species
            <select value={form.species} onChange={e => setForm(f => ({ ...f, species: e.target.value as typeof form.species }))}
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }}>
              {SPECIES.map(s => <option key={s}>{s}</option>)}
            </select>
          </label>
          <label>
            Health Status
            <select value={form.healthStatus} onChange={e => setForm(f => ({ ...f, healthStatus: e.target.value as typeof form.healthStatus }))}
              style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px' }}>
              {HEALTH.map(h => <option key={h}>{h}</option>)}
            </select>
          </label>
          <label style={{ gridColumn: '1 / -1' }}>
            Description
            <textarea value={form.description ?? ''} onChange={e => setForm(f => ({ ...f, description: e.target.value || undefined }))}
              rows={2} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box' }} />
          </label>
          <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
            <button type="submit" style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>Save</button>
            <button type="button" onClick={() => setShowForm(false)} style={{ padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>Cancel</button>
          </div>
        </form>
      )}

      {loading ? <p>Loading…</p> : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ background: '#d8f3dc', textAlign: 'left' }}>
              {['Identifier','Name','Species','Health','Last Seen','Actions'].map(h => (
                <th key={h} style={{ padding: '8px 12px', borderBottom: '2px solid #2d6a4f' }}>{h}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {animals.map(a => (
              <Fragment key={a.id}>
                <tr style={{ borderBottom: movementAnimalId === a.id ? 'none' : '1px solid #eee', background: movementAnimalId === a.id ? '#f0f7f0' : 'transparent' }}>
                  <td style={{ padding: '8px 12px' }}>{a.identifier}</td>
                  <td style={{ padding: '8px 12px' }}>{a.name}</td>
                  <td style={{ padding: '8px 12px' }}>{a.species}</td>
                  <td style={{ padding: '8px 12px' }}>{a.healthStatus}</td>
                  <td style={{ padding: '8px 12px' }}>{a.lastSeenAtUtc ? new Date(a.lastSeenAtUtc).toLocaleDateString() : '—'}</td>
                  <td style={{ padding: '8px 12px', display: 'flex', gap: '0.4rem' }}>
                    <button onClick={() => editingId === a.id ? setEditingId(null) : openEdit(a)}
                      style={{ background: editingId === a.id ? '#374151' : '#e2e8f0', color: editingId === a.id ? '#fff' : '#333', border: 'none', padding: '4px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                      {editingId === a.id ? 'Cancel' : 'Edit'}
                    </button>
                    <button onClick={() => toggleMovement(a.id)}
                      style={{ background: movementAnimalId === a.id ? '#2d6a4f' : '#e2e8f0', color: movementAnimalId === a.id ? '#fff' : '#333', border: 'none', padding: '4px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                      {movementAnimalId === a.id ? 'Hide Trail' : 'Trail'}
                    </button>
                    <button onClick={() => handleDelete(a.id)}
                      style={{ background: '#c53030', color: '#fff', border: 'none', padding: '4px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                      Delete
                    </button>
                  </td>
                </tr>

                {editingId === a.id && (
                  <tr style={{ background: '#f8fdf8' }}>
                    <td colSpan={6} style={{ padding: '0 12px 12px' }}>
                      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0.75rem', paddingTop: '0.75rem' }}>
                        <label style={{ fontSize: '0.9rem' }}>
                          Name *
                          <input value={editForm.name} onChange={e => setEditForm(f => ({ ...f, name: e.target.value }))} required
                            style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', borderRadius: '4px', border: '1px solid #ccc' }} />
                        </label>
                        <label style={{ fontSize: '0.9rem' }}>
                          Species
                          <select value={editForm.species} onChange={e => setEditForm(f => ({ ...f, species: e.target.value as typeof SPECIES[number] }))}
                            style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #ccc' }}>
                            {SPECIES.map(s => <option key={s}>{s}</option>)}
                          </select>
                        </label>
                        <label style={{ fontSize: '0.9rem' }}>
                          Health Status
                          <select value={editForm.healthStatus} onChange={e => setEditForm(f => ({ ...f, healthStatus: e.target.value as typeof HEALTH[number] }))}
                            style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #ccc' }}>
                            {HEALTH.map(h => <option key={h}>{h}</option>)}
                          </select>
                        </label>
                        <label style={{ fontSize: '0.9rem' }}>
                          Description
                          <input value={editForm.description} onChange={e => setEditForm(f => ({ ...f, description: e.target.value }))}
                            style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', borderRadius: '4px', border: '1px solid #ccc' }} />
                        </label>
                        <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
                          <button onClick={() => handleSaveEdit(a.id)}
                            style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '7px 16px', cursor: 'pointer', borderRadius: '4px' }}>
                            Save
                          </button>
                          <button onClick={() => setEditingId(null)}
                            style={{ padding: '7px 16px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #ccc', background: '#fff' }}>
                            Cancel
                          </button>
                        </div>
                      </div>
                    </td>
                  </tr>
                )}

                {movementAnimalId === a.id && (
                  <tr style={{ background: '#f0f7f0' }}>
                    <td colSpan={6} style={{ padding: '0 12px 12px' }}>
                      <MovementPanel
                        animal={movementAnimal!}
                        points={movement}
                        loading={movementLoading}
                        error={movementError}
                      />
                    </td>
                  </tr>
                )}
              </Fragment>
            ))}
            {animals.length === 0 && (
              <tr><td colSpan={6} style={{ padding: '2rem', textAlign: 'center', color: '#666' }}>No animals found.</td></tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  )
}

// ── Movement panel ────────────────────────────────────────────────────────────

interface MovementPanelProps {
  animal: AnimalDto
  points: MovementPointDto[]
  loading: boolean
  error: string | null
}

function MovementPanel({ animal, points, loading, error }: MovementPanelProps) {
  if (loading) return <p style={{ color: '#666', margin: '0.5rem 0' }}>Loading movement history…</p>
  if (error)   return <p style={{ color: '#c53030', margin: '0.5rem 0' }}>{error}</p>

  return (
    <div>
      <p style={{ margin: '0.5rem 0', fontWeight: 600, color: '#2d6a4f' }}>
        Movement trail — {animal.name} ({animal.identifier})
        <span style={{ fontWeight: 400, color: '#555', marginLeft: '0.5rem' }}>
          {points.length} sighting{points.length !== 1 ? 's' : ''}
        </span>
      </p>

      {points.length === 0 ? (
        <p style={{ color: '#666', margin: '0.25rem 0' }}>No sightings recorded yet.</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.875rem' }}>
          <thead>
            <tr style={{ background: '#d8f3dc', textAlign: 'left' }}>
              {['Date','Coordinates','Region','Forest District','Type','Status'].map(h => (
                <th key={h} style={{ padding: '6px 10px', borderBottom: '1px solid #2d6a4f' }}>{h}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {points.map((p, i) => (
              <tr key={p.reportId} style={{ borderBottom: '1px solid #e2e8f0', background: i % 2 === 0 ? '#fff' : '#f8fdf8' }}>
                <td style={{ padding: '6px 10px', whiteSpace: 'nowrap' }}>
                  {new Date(p.observedAtUtc).toLocaleString()}
                </td>
                <td style={{ padding: '6px 10px', fontFamily: 'monospace', whiteSpace: 'nowrap' }}>
                  {p.latitude.toFixed(5)}, {p.longitude.toFixed(5)}
                </td>
                <td style={{ padding: '6px 10px' }}>{p.region ?? '—'}</td>
                <td style={{ padding: '6px 10px' }}>{p.forestDistrict ?? '—'}</td>
                <td style={{ padding: '6px 10px' }}>{p.reportType}</td>
                <td style={{ padding: '6px 10px' }}>
                  <span style={{ color: STATUS_COLOR[p.status] ?? '#333', fontWeight: 600 }}>
                    {p.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}
