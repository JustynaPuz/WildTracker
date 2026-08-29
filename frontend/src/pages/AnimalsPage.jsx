import { useEffect, useState, useCallback, Fragment } from 'react';
import { animalsApi } from '../api/animals';
import { STATUS_COLOR } from '../constants';
import { useAuth } from '../context/AuthContext';
const SPECIES = ['Unknown', 'Wolf', 'Fox', 'Bear', 'Deer', 'Boar', 'Lynx', 'Moose', 'Bird', 'Other'];
const HEALTH = ['Unknown', 'Healthy', 'Injured', 'Sick', 'Dead'];
export default function AnimalsPage() {
    const { user } = useAuth();
    const canWrite = user?.role === 'Ranger' || user?.role === 'Admin';
    const isAdmin = user?.role === 'Admin';
    const [animals, setAnimals] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showForm, setShowForm] = useState(false);
    const [form, setForm] = useState({
        identifier: '', name: '', species: 'Unknown', healthStatus: 'Unknown',
    });
    const [editingId, setEditingId] = useState(null);
    const [editForm, setEditForm] = useState({ name: '', species: 'Unknown', healthStatus: 'Unknown', description: '' });
    // movement panel state
    const [movementAnimalId, setMovementAnimalId] = useState(null);
    const [movement, setMovement] = useState([]);
    const [movementLoading, setMovementLoading] = useState(false);
    const [movementError, setMovementError] = useState(null);
    // filters
    const [filterSpecies, setFilterSpecies] = useState('');
    const [filterHealth, setFilterHealth] = useState('');
    const [searchInput, setSearchInput] = useState('');
    const [searchTerm, setSearchTerm] = useState('');
    // pagination
    const [page, setPage] = useState(1);
    const [total, setTotal] = useState(0);
    const PAGE_SIZE = 12;
    useEffect(() => {
        const t = setTimeout(() => setSearchTerm(searchInput.trim()), 300);
        return () => clearTimeout(t);
    }, [searchInput]);
    useEffect(() => { setPage(1); }, [filterSpecies, filterHealth, searchTerm]);
    const load = useCallback(() => {
        setLoading(true);
        animalsApi.search({
            species: filterSpecies || undefined,
            healthStatus: filterHealth || undefined,
            searchTerm: searchTerm || undefined,
            page,
            pageSize: PAGE_SIZE,
        })
            .then((r) => { setAnimals(r.items); setTotal(r.totalCount); })
            .catch(() => setError('Failed to load animals.'))
            .finally(() => setLoading(false));
    }, [filterSpecies, filterHealth, searchTerm, page]);
    useEffect(() => { load(); }, [load]);
    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            await animalsApi.create(form);
            setShowForm(false);
            setForm({ identifier: '', name: '', species: 'Unknown', healthStatus: 'Unknown' });
            load();
        }
        catch {
            setError('Failed to create animal.');
        }
    };
    const handleDelete = async (id) => {
        if (!confirm('Delete this animal?'))
            return;
        try {
            await animalsApi.delete(id);
            if (movementAnimalId === id)
                setMovementAnimalId(null);
            load();
        }
        catch {
            setError('Failed to delete animal.');
        }
    };
    const openEdit = (a) => {
        setEditingId(a.id);
        setEditForm({ name: a.name, species: a.species, healthStatus: a.healthStatus, description: a.description ?? '' });
        setMovementAnimalId(null);
    };
    const handleSaveEdit = async (id) => {
        try {
            await animalsApi.update(id, {
                name: editForm.name,
                species: editForm.species,
                healthStatus: editForm.healthStatus,
                description: editForm.description || undefined,
            });
            setEditingId(null);
            load();
        }
        catch {
            setError('Failed to update animal.');
        }
    };
    const toggleMovement = async (id) => {
        if (movementAnimalId === id) {
            setMovementAnimalId(null);
            return;
        }
        setEditingId(null);
        setMovementAnimalId(id);
        setMovement([]);
        setMovementError(null);
        setMovementLoading(true);
        try {
            const pts = await animalsApi.getMovement(id);
            setMovement(pts);
        }
        catch {
            setMovementError('Failed to load movement history.');
        }
        finally {
            setMovementLoading(false);
        }
    };
    const movementAnimal = animals.find(a => a.id === movementAnimalId);
    const totalPages = Math.ceil(total / PAGE_SIZE);
    return (<div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
        <h1 style={{ color: '#34d399', margin: 0 }}>Animals</h1>
        {canWrite && (<button onClick={() => setShowForm(!showForm)} style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>
          + Add Animal
        </button>)}
      </div>

      {error && <p style={{ color: '#f87171' }}>{error}</p>}

      {/* Filters */}
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', flexWrap: 'wrap', alignItems: 'center' }}>
        <label>
          Species&nbsp;
          <select value={filterSpecies} onChange={(e) => setFilterSpecies(e.target.value)} style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
            <option value="">All species</option>
            {SPECIES.map((s) => <option key={s} value={s}>{s}</option>)}
          </select>
        </label>
        <label>
          Health&nbsp;
          <select value={filterHealth} onChange={(e) => setFilterHealth(e.target.value)} style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
            <option value="">All health statuses</option>
            {HEALTH.map((h) => <option key={h} value={h}>{h}</option>)}
          </select>
        </label>
        <label style={{ flex: '1 1 200px', minWidth: '180px' }}>
          Search&nbsp;
          <input value={searchInput} onChange={(e) => setSearchInput(e.target.value)} placeholder="Name or identifier…" style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8', width: '100%', maxWidth: '260px', boxSizing: 'border-box' }}/>
        </label>
      </div>

      {showForm && (<form onSubmit={handleCreate} style={{ background: '#1a1a1a', border: '1px solid #2a2a2a', padding: '1rem', borderRadius: '8px', marginBottom: '1.5rem', display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0.75rem' }}>
          <label>
            Identifier *
            <input value={form.identifier} onChange={e => setForm(f => ({ ...f, identifier: e.target.value }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label>
            Name *
            <input value={form.name} onChange={e => setForm(f => ({ ...f, name: e.target.value }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label>
            Species
            <select value={form.species} onChange={e => setForm(f => ({ ...f, species: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}>
              {SPECIES.map(s => <option key={s}>{s}</option>)}
            </select>
          </label>
          <label>
            Health Status
            <select value={form.healthStatus} onChange={e => setForm(f => ({ ...f, healthStatus: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}>
              {HEALTH.map(h => <option key={h}>{h}</option>)}
            </select>
          </label>
          <label style={{ gridColumn: '1 / -1' }}>
            Description
            <textarea value={form.description ?? ''} onChange={e => setForm(f => ({ ...f, description: e.target.value || undefined }))} rows={2} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
            <button type="submit" style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>Save</button>
            <button type="button" onClick={() => setShowForm(false)} style={{ padding: '8px 16px', cursor: 'pointer', borderRadius: '4px', background: '#2a2a2a', color: '#e8e8e8', border: '1px solid #3a3a3a' }}>Cancel</button>
          </div>
        </form>)}

      {loading ? <p>Loading…</p> : (<>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ background: '#1f3d2f', textAlign: 'left' }}>
              {['Identifier', 'Name', 'Species', 'Health', 'Last Seen', 'Actions'].map(h => (<th key={h} style={{ padding: '8px 12px', borderBottom: '2px solid #2d6a4f' }}>{h}</th>))}
            </tr>
          </thead>
          <tbody>
            {animals.map(a => (<Fragment key={a.id}>
                <tr style={{ borderBottom: movementAnimalId === a.id ? 'none' : '1px solid #2a2a2a', background: movementAnimalId === a.id ? '#1a1a1a' : 'transparent' }}>
                  <td style={{ padding: '8px 12px' }}>{a.identifier}</td>
                  <td style={{ padding: '8px 12px' }}>{a.name}</td>
                  <td style={{ padding: '8px 12px' }}>{a.species}</td>
                  <td style={{ padding: '8px 12px' }}>{a.healthStatus}</td>
                  <td style={{ padding: '8px 12px' }}>{a.lastSeenAtUtc ? new Date(a.lastSeenAtUtc).toLocaleDateString() : '—'}</td>
                  <td style={{ padding: '8px 12px', display: 'flex', gap: '0.4rem' }}>
                    {canWrite && (<button onClick={() => editingId === a.id ? setEditingId(null) : openEdit(a)} style={{ background: editingId === a.id ? '#374151' : '#2e2e2e', color: editingId === a.id ? '#fff' : '#e5e5e5', border: 'none', padding: '4px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                      {editingId === a.id ? 'Cancel' : 'Edit'}
                    </button>)}
                    <button onClick={() => toggleMovement(a.id)} style={{ background: movementAnimalId === a.id ? '#2d6a4f' : '#2e2e2e', color: movementAnimalId === a.id ? '#fff' : '#e5e5e5', border: 'none', padding: '4px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                      {movementAnimalId === a.id ? 'Hide Trail' : 'Trail'}
                    </button>
                    {isAdmin && (<button onClick={() => handleDelete(a.id)} style={{ background: '#c53030', color: '#fff', border: 'none', padding: '4px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                      Delete
                    </button>)}
                  </td>
                </tr>

                {editingId === a.id && (<tr style={{ background: '#181818' }}>
                    <td colSpan={6} style={{ padding: '0 12px 12px' }}>
                      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0.75rem', paddingTop: '0.75rem' }}>
                        <label style={{ fontSize: '0.9rem' }}>
                          Name *
                          <input value={editForm.name} onChange={e => setEditForm(f => ({ ...f, name: e.target.value }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                        </label>
                        <label style={{ fontSize: '0.9rem' }}>
                          Species
                          <select value={editForm.species} onChange={e => setEditForm(f => ({ ...f, species: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
                            {SPECIES.map(s => <option key={s}>{s}</option>)}
                          </select>
                        </label>
                        <label style={{ fontSize: '0.9rem' }}>
                          Health Status
                          <select value={editForm.healthStatus} onChange={e => setEditForm(f => ({ ...f, healthStatus: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
                            {HEALTH.map(h => <option key={h}>{h}</option>)}
                          </select>
                        </label>
                        <label style={{ fontSize: '0.9rem' }}>
                          Description
                          <input value={editForm.description} onChange={e => setEditForm(f => ({ ...f, description: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', boxSizing: 'border-box', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                        </label>
                        <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
                          <button onClick={() => handleSaveEdit(a.id)} style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '7px 16px', cursor: 'pointer', borderRadius: '4px' }}>
                            Save
                          </button>
                          <button onClick={() => setEditingId(null)} style={{ padding: '7px 16px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#2a2a2a', color: '#e8e8e8' }}>
                            Cancel
                          </button>
                        </div>
                      </div>
                    </td>
                  </tr>)}

                {movementAnimalId === a.id && (<tr style={{ background: '#1a1a1a' }}>
                    <td colSpan={6} style={{ padding: '0 12px 12px' }}>
                      <MovementPanel animal={movementAnimal} points={movement} loading={movementLoading} error={movementError}/>
                    </td>
                  </tr>)}
              </Fragment>))}
            {animals.length === 0 && (<tr><td colSpan={6} style={{ padding: '2rem', textAlign: 'center', color: '#8f8f8f' }}>No animals found.</td></tr>)}
          </tbody>
          </table>

          {totalPages > 1 && (<div style={{ display: 'flex', gap: '0.5rem', justifyContent: 'center', marginTop: '1rem' }}>
              <button onClick={() => setPage((p) => Math.max(1, p - 1))} disabled={page === 1} style={{ padding: '6px 14px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#1e1e1e', color: '#e8e8e8' }}>
                Prev
              </button>
              <span style={{ lineHeight: '34px', fontSize: '0.9rem' }}>{page} / {totalPages}</span>
              <button onClick={() => setPage((p) => Math.min(totalPages, p + 1))} disabled={page === totalPages} style={{ padding: '6px 14px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#1e1e1e', color: '#e8e8e8' }}>
                Next
              </button>
            </div>)}
        </>)}
    </div>);
}
function MovementPanel({ animal, points, loading, error }) {
    if (loading)
        return <p style={{ color: '#8f8f8f', margin: '0.5rem 0' }}>Loading movement history…</p>;
    if (error)
        return <p style={{ color: '#f87171', margin: '0.5rem 0' }}>{error}</p>;
    return (<div>
      <p style={{ margin: '0.5rem 0', fontWeight: 600, color: '#34d399' }}>
        Movement trail — {animal.name} ({animal.identifier})
        <span style={{ fontWeight: 400, color: '#b3b3b3', marginLeft: '0.5rem' }}>
          {points.length} sighting{points.length !== 1 ? 's' : ''}
        </span>
      </p>

      {points.length === 0 ? (<p style={{ color: '#8f8f8f', margin: '0.25rem 0' }}>No sightings recorded yet.</p>) : (<table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.875rem' }}>
          <thead>
            <tr style={{ background: '#1f3d2f', textAlign: 'left' }}>
              {['Date', 'Coordinates', 'Region', 'Forest District', 'Type', 'Status'].map(h => (<th key={h} style={{ padding: '6px 10px', borderBottom: '1px solid #2d6a4f' }}>{h}</th>))}
            </tr>
          </thead>
          <tbody>
            {points.map((p, i) => (<tr key={p.reportId} style={{ borderBottom: '1px solid #2a2a2a', background: i % 2 === 0 ? '#1a1a1a' : '#181818' }}>
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
              </tr>))}
          </tbody>
        </table>)}
    </div>);
}
