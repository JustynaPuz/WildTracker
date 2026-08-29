import { useCallback, useEffect, useState, Fragment } from 'react';
import { reportsApi } from '../api/reports';
import { animalsApi } from '../api/animals';
import { notesApi } from '../api/notes';
import { STATUS_COLOR } from '../constants';
import { useAuth } from '../context/AuthContext';
const REPORT_TYPES = ['Sighting', 'Injury', 'Death', 'DangerousBehavior', 'Other'];
const SOURCES = ['Manual', 'CameraTrap', 'Drone', 'Sensor', 'Imported'];
const STATUSES = ['Pending', 'Verified', 'Rejected', 'Resolved'];
const SPECIES = ['Unknown', 'Wolf', 'Fox', 'Bear', 'Deer', 'Boar', 'Lynx', 'Moose', 'Bird', 'Other'];
const btn = (bg) => ({
    background: bg, color: '#fff', border: 'none',
    padding: '3px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem',
});
const emptyForm = () => ({
    animalId: '',
    observedAtUtc: new Date().toISOString().slice(0, 16),
    reportType: 'Sighting',
    source: 'Manual',
    latitude: 0,
    longitude: 0,
    region: '',
    forestDistrict: '',
    description: '',
});
export default function ReportsPage() {
    const { user } = useAuth();
    const canWrite = user?.role === 'Ranger' || user?.role === 'Admin';
    const isAdmin = user?.role === 'Admin';
    const [animals, setAnimals] = useState([]);
    const [reports, setReports] = useState([]);
    const [total, setTotal] = useState(0);
    const [page, setPage] = useState(1);
    const PAGE_SIZE = 12;
    const [animalSearchInput, setAnimalSearchInput] = useState('');
    const [filterAnimalTerm, setFilterAnimalTerm] = useState('');
    const [filterStatus, setFilterStatus] = useState('');
    const [filterSpecies, setFilterSpecies] = useState('');
    const [filterFrom, setFilterFrom] = useState('');
    const [filterTo, setFilterTo] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showForm, setShowForm] = useState(false);
    const [form, setForm] = useState(emptyForm());
    const [expandedId, setExpandedId] = useState(null);
    const [notes, setNotes] = useState({});
    const [noteContent, setNoteContent] = useState('');
    // per-note edit state: noteId → draft content (undefined = not editing)
    const [editingNote, setEditingNote] = useState({});
    // report edit state
    const [editingReportId, setEditingReportId] = useState(null);
    const [editReportForm, setEditReportForm] = useState({});
    useEffect(() => { animalsApi.getAll().then(setAnimals).catch(() => { }); }, []);
    useEffect(() => {
        const t = setTimeout(() => setFilterAnimalTerm(animalSearchInput.trim()), 300);
        return () => clearTimeout(t);
    }, [animalSearchInput]);
    useEffect(() => { setPage(1); }, [filterAnimalTerm]);
    const loadReports = useCallback(() => {
        setLoading(true);
        reportsApi.search({
            animalSearchTerm: filterAnimalTerm || undefined,
            status: filterStatus || undefined,
            species: filterSpecies || undefined,
            from: filterFrom ? new Date(`${filterFrom}T00:00:00`).toISOString() : undefined,
            to: filterTo ? new Date(`${filterTo}T23:59:59`).toISOString() : undefined,
            page,
            pageSize: PAGE_SIZE,
        })
            .then((r) => { setReports(r.items); setTotal(r.totalCount); })
            .catch(() => setError('Failed to load reports.'))
            .finally(() => setLoading(false));
    }, [filterAnimalTerm, filterStatus, filterSpecies, filterFrom, filterTo, page]);
    useEffect(() => { loadReports(); }, [loadReports]);
    // ── report actions ────────────────────────────────────────────────────────
    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            await reportsApi.create({
                ...form,
                observedAtUtc: new Date(form.observedAtUtc).toISOString(),
                region: form.region || undefined,
                forestDistrict: form.forestDistrict || undefined,
                description: form.description || undefined,
            });
            setShowForm(false);
            setForm(emptyForm());
            loadReports();
        }
        catch {
            setError('Failed to create report.');
        }
    };
    const handleApprove = async (id) => {
        try {
            await reportsApi.approve(id);
            loadReports();
        }
        catch {
            setError('Failed to approve.');
        }
    };
    const handleReject = async (id) => {
        try {
            await reportsApi.reject(id);
            loadReports();
        }
        catch {
            setError('Failed to reject.');
        }
    };
    const handleResolve = async (id) => {
        try {
            await reportsApi.resolve(id);
            loadReports();
        }
        catch {
            setError('Failed to resolve.');
        }
    };
    const handleDeleteReport = async (id) => {
        if (!confirm('Delete this report?'))
            return;
        try {
            await reportsApi.delete(id);
            loadReports();
        }
        catch {
            setError('Failed to delete.');
        }
    };
    const openEditReport = (r) => {
        setEditingReportId(r.id);
        setEditReportForm({
            observedAtUtc: new Date(r.observedAtUtc).toISOString().slice(0, 16),
            reportType: r.reportType,
            source: r.source,
            latitude: r.location.latitude,
            longitude: r.location.longitude,
            region: r.location.region ?? '',
            forestDistrict: r.location.forestDistrict ?? '',
            description: r.description ?? '',
        });
        setExpandedId(null);
    };
    const handleSaveReport = async (id) => {
        try {
            await reportsApi.update(id, {
                observedAtUtc: new Date(editReportForm.observedAtUtc).toISOString(),
                reportType: editReportForm.reportType,
                source: editReportForm.source,
                latitude: Number(editReportForm.latitude),
                longitude: Number(editReportForm.longitude),
                region: editReportForm.region || undefined,
                forestDistrict: editReportForm.forestDistrict || undefined,
                description: editReportForm.description || undefined,
            });
            setEditingReportId(null);
            loadReports();
        }
        catch {
            setError('Failed to update report.');
        }
    };
    // ── note actions ──────────────────────────────────────────────────────────
    const reloadNotes = async (reportId) => {
        const data = await notesApi.getByReport(reportId).catch(() => []);
        setNotes((prev) => ({ ...prev, [reportId]: data }));
    };
    const toggleNotes = async (id) => {
        if (expandedId === id) {
            setExpandedId(null);
            return;
        }
        setEditingReportId(null);
        setExpandedId(id);
        if (!notes[id])
            await reloadNotes(id);
    };
    const handleAddNote = async (reportId) => {
        if (!noteContent.trim())
            return;
        try {
            await notesApi.create(reportId, { content: noteContent });
            setNoteContent('');
            await reloadNotes(reportId);
        }
        catch {
            setError('Failed to add note.');
        }
    };
    const handleEditNote = (noteId, current) => setEditingNote((prev) => ({ ...prev, [noteId]: current }));
    const handleCancelEdit = (noteId) => setEditingNote((prev) => { const next = { ...prev }; delete next[noteId]; return next; });
    const handleSaveNote = async (reportId, noteId) => {
        const content = editingNote[noteId]?.trim();
        if (!content)
            return;
        try {
            await notesApi.update(reportId, noteId, { content });
            handleCancelEdit(noteId);
            await reloadNotes(reportId);
        }
        catch {
            setError('Failed to update note.');
        }
    };
    const handleDeleteNote = async (reportId, noteId) => {
        if (!confirm('Delete this note?'))
            return;
        try {
            await notesApi.delete(reportId, noteId);
            await reloadNotes(reportId);
        }
        catch {
            setError('Failed to delete note.');
        }
    };
    // ── helpers ───────────────────────────────────────────────────────────────
    const totalPages = Math.ceil(total / PAGE_SIZE);
    const animalName = (id) => animals.find((a) => a.id === id)?.name ?? id.slice(0, 8);
    return (<div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
        <h1 style={{ color: '#34d399', margin: 0 }}>Sighting Reports</h1>
        {canWrite && (<button onClick={() => setShowForm(!showForm)} style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>
          + New Report
        </button>)}
      </div>

      {error && <p style={{ color: '#f87171' }}>{error}</p>}

      {/* Filters */}
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', flexWrap: 'wrap', alignItems: 'center' }}>
        <label>
          Animal&nbsp;
          <input value={animalSearchInput} onChange={(e) => setAnimalSearchInput(e.target.value)} placeholder="Name or identifier…" style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
        </label>
        <label>
          Species&nbsp;
          <select value={filterSpecies} onChange={(e) => { setFilterSpecies(e.target.value); setPage(1); }} style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
            <option value="">All species</option>
            {SPECIES.map((s) => <option key={s} value={s}>{s}</option>)}
          </select>
        </label>
        <label>
          Status&nbsp;
          <select value={filterStatus} onChange={(e) => { setFilterStatus(e.target.value); setPage(1); }} style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
            <option value="">All statuses</option>
            {STATUSES.map((s) => <option key={s}>{s}</option>)}
          </select>
        </label>
        <label>
          From&nbsp;
          <input type="date" value={filterFrom} onChange={(e) => { setFilterFrom(e.target.value); setPage(1); }} style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
        </label>
        <label>
          To&nbsp;
          <input type="date" value={filterTo} onChange={(e) => { setFilterTo(e.target.value); setPage(1); }} style={{ padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
        </label>
        {(animalSearchInput || filterSpecies || filterStatus || filterFrom || filterTo) && (
          <button onClick={() => { setAnimalSearchInput(''); setFilterAnimalTerm(''); setFilterSpecies(''); setFilterStatus(''); setFilterFrom(''); setFilterTo(''); setPage(1); }} style={{ padding: '6px 12px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#2a2a2a', color: '#e8e8e8', cursor: 'pointer', fontSize: '0.85rem' }}>
            Clear filters
          </button>
        )}
      </div>

      {/* Create form */}
      {showForm && (<form onSubmit={handleCreate} style={{ background: '#1a1a1a', border: '1px solid #2a2a2a', padding: '1rem', borderRadius: '8px', marginBottom: '1.5rem', display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '0.75rem' }}>
          <label>
            Animal *
            <select value={form.animalId} onChange={(e) => setForm((f) => ({ ...f, animalId: e.target.value }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}>
              <option value="">Select…</option>
              {animals.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
            </select>
          </label>
          <label>
            Observed At *
            <input type="datetime-local" value={form.observedAtUtc} onChange={(e) => setForm((f) => ({ ...f, observedAtUtc: e.target.value }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label>
            Report Type
            <select value={form.reportType} onChange={(e) => setForm((f) => ({ ...f, reportType: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}>
              {REPORT_TYPES.map((t) => <option key={t}>{t}</option>)}
            </select>
          </label>
          <label>
            Source
            <select value={form.source} onChange={(e) => setForm((f) => ({ ...f, source: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}>
              {SOURCES.map((s) => <option key={s}>{s}</option>)}
            </select>
          </label>
          <label>
            Latitude *
            <input type="number" step="any" value={form.latitude} onChange={(e) => setForm((f) => ({ ...f, latitude: parseFloat(e.target.value) || 0 }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label>
            Longitude *
            <input type="number" step="any" value={form.longitude} onChange={(e) => setForm((f) => ({ ...f, longitude: parseFloat(e.target.value) || 0 }))} required style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label>
            Region
            <input value={form.region ?? ''} onChange={(e) => setForm((f) => ({ ...f, region: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label>
            Forest District
            <input value={form.forestDistrict ?? ''} onChange={(e) => setForm((f) => ({ ...f, forestDistrict: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <label style={{ gridColumn: '1 / -1' }}>
            Description
            <textarea value={form.description ?? ''} onChange={(e) => setForm((f) => ({ ...f, description: e.target.value }))} rows={2} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', background: '#141414', color: '#e8e8e8', border: '1px solid #3a3a3a', borderRadius: '4px' }}/>
          </label>
          <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
            <button type="submit" style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '8px 16px', cursor: 'pointer', borderRadius: '4px' }}>Save</button>
            <button type="button" onClick={() => setShowForm(false)} style={{ padding: '8px 16px', cursor: 'pointer', borderRadius: '4px', background: '#2a2a2a', color: '#e8e8e8', border: '1px solid #3a3a3a' }}>Cancel</button>
          </div>
        </form>)}

      {/* Reports table */}
      {loading ? <p>Loading…</p> : (<>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead>
              <tr style={{ background: '#1f3d2f', textAlign: 'left' }}>
                {['Animal', 'Type', 'Source', 'Location', 'Observed', 'Status', 'Actions'].map((h) => (<th key={h} style={{ padding: '8px 12px', borderBottom: '2px solid #2d6a4f' }}>{h}</th>))}
              </tr>
            </thead>
            <tbody>
              {reports.map((r) => (<Fragment key={r.id}>
                  <tr style={{ borderBottom: expandedId === r.id ? 'none' : '1px solid #2a2a2a' }}>
                    <td style={{ padding: '8px 12px' }}>{animalName(r.animalId)}</td>
                    <td style={{ padding: '8px 12px' }}>{r.reportType}</td>
                    <td style={{ padding: '8px 12px' }}>{r.source}</td>
                    <td style={{ padding: '8px 12px' }}>
                      {r.location.latitude.toFixed(4)}, {r.location.longitude.toFixed(4)}
                      {r.location.region && <span style={{ color: '#9a9a9a', fontSize: '0.8rem' }}> {r.location.region}</span>}
                    </td>
                    <td style={{ padding: '8px 12px' }}>{new Date(r.observedAtUtc).toLocaleDateString()}</td>
                    <td style={{ padding: '8px 12px' }}>
                      <span style={{ color: STATUS_COLOR[r.status], fontWeight: 600 }}>{r.status}</span>
                    </td>
                    <td style={{ padding: '8px 12px' }}>
                      <div style={{ display: 'flex', gap: '4px', flexWrap: 'wrap' }}>
                        {canWrite && r.status === 'Pending' && (<>
                            <button onClick={() => handleApprove(r.id)} style={btn('#2d6a4f')}>Approve</button>
                            <button onClick={() => handleReject(r.id)} style={btn('#c53030')}>Reject</button>
                          </>)}
                        {canWrite && r.status === 'Verified' && (<button onClick={() => handleResolve(r.id)} style={btn('#6b7280')}>Resolve</button>)}
                        {canWrite && (<button onClick={() => editingReportId === r.id ? setEditingReportId(null) : openEditReport(r)} style={btn(editingReportId === r.id ? '#374151' : '#374151')}>
                          {editingReportId === r.id ? 'Cancel' : 'Edit'}
                        </button>)}
                        <button onClick={() => toggleNotes(r.id)} style={btn('#374151')}>
                          {expandedId === r.id ? 'Hide Notes' : 'Notes'}
                        </button>
                        {isAdmin && (<button onClick={() => handleDeleteReport(r.id)} style={btn('#c53030')}>Delete</button>)}
                      </div>
                    </td>
                  </tr>

                  {editingReportId === r.id && (<tr>
                      <td colSpan={7} style={{ padding: '0 12px 12px', background: '#181818', borderBottom: '1px solid #2a2a2a' }}>
                        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '0.75rem', paddingTop: '0.75rem' }}>
                          <label style={{ fontSize: '0.9rem' }}>
                            Observed At
                            <input type="datetime-local" value={editReportForm.observedAtUtc ?? ''} onChange={e => setEditReportForm(f => ({ ...f, observedAtUtc: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                          </label>
                          <label style={{ fontSize: '0.9rem' }}>
                            Report Type
                            <select value={editReportForm.reportType ?? 'Sighting'} onChange={e => setEditReportForm(f => ({ ...f, reportType: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
                              {REPORT_TYPES.map(t => <option key={t}>{t}</option>)}
                            </select>
                          </label>
                          <label style={{ fontSize: '0.9rem' }}>
                            Source
                            <select value={editReportForm.source ?? 'Manual'} onChange={e => setEditReportForm(f => ({ ...f, source: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
                              {SOURCES.map(s => <option key={s}>{s}</option>)}
                            </select>
                          </label>
                          <label style={{ fontSize: '0.9rem' }}>
                            Latitude
                            <input type="number" step="any" value={editReportForm.latitude ?? 0} onChange={e => setEditReportForm(f => ({ ...f, latitude: parseFloat(e.target.value) || 0 }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                          </label>
                          <label style={{ fontSize: '0.9rem' }}>
                            Longitude
                            <input type="number" step="any" value={editReportForm.longitude ?? 0} onChange={e => setEditReportForm(f => ({ ...f, longitude: parseFloat(e.target.value) || 0 }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                          </label>
                          <label style={{ fontSize: '0.9rem' }}>
                            Region
                            <input value={editReportForm.region ?? ''} onChange={e => setEditReportForm(f => ({ ...f, region: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                          </label>
                          <label style={{ fontSize: '0.9rem' }}>
                            Forest District
                            <input value={editReportForm.forestDistrict ?? ''} onChange={e => setEditReportForm(f => ({ ...f, forestDistrict: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                          </label>
                          <label style={{ fontSize: '0.9rem', gridColumn: '2 / -1' }}>
                            Description
                            <input value={editReportForm.description ?? ''} onChange={e => setEditReportForm(f => ({ ...f, description: e.target.value }))} style={{ display: 'block', width: '100%', padding: '6px', marginTop: '2px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                          </label>
                          <div style={{ gridColumn: '1 / -1', display: 'flex', gap: '0.5rem' }}>
                            <button onClick={() => handleSaveReport(r.id)} style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '7px 16px', cursor: 'pointer', borderRadius: '4px' }}>
                              Save
                            </button>
                            <button onClick={() => setEditingReportId(null)} style={{ padding: '7px 16px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#2a2a2a', color: '#e8e8e8' }}>
                              Cancel
                            </button>
                          </div>
                        </div>
                      </td>
                    </tr>)}

                  {expandedId === r.id && (<tr key={`${r.id}-notes`}>
                      <td colSpan={7} style={{ padding: '0 12px 12px 12px', background: '#161f1c', borderBottom: '1px solid #2a2a2a' }}>
                        <div style={{ paddingTop: '8px' }}>
                          {(notes[r.id] ?? []).length === 0
                        ? <p style={{ color: '#8f8f8f', margin: '0 0 8px 0' }}>No notes yet.</p>
                        : (notes[r.id] ?? []).map((n) => (<div key={n.id} style={{ marginBottom: '8px', padding: '8px', background: '#1e1e1e', borderRadius: '4px', border: '1px solid #2a2a2a' }}>
                                  {editingNote[n.id] !== undefined ? (
                            /* ── edit mode ── */
                            <div>
                                      <textarea value={editingNote[n.id]} onChange={(e) => setEditingNote((prev) => ({ ...prev, [n.id]: e.target.value }))} rows={3} style={{ width: '100%', padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8', boxSizing: 'border-box' }}/>
                                      <div style={{ display: 'flex', gap: '6px', marginTop: '4px' }}>
                                        <button onClick={() => handleSaveNote(r.id, n.id)} style={btn('#2d6a4f')}>Save</button>
                                        <button onClick={() => handleCancelEdit(n.id)} style={{ border: '1px solid #3a3a3a', background: '#2a2a2a', color: '#e8e8e8', padding: '3px 8px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.8rem' }}>
                                          Cancel
                                        </button>
                                      </div>
                                    </div>) : (
                            /* ── read mode ── */
                            <div>
                                      <p style={{ margin: '0 0 4px 0' }}>{n.content}</p>
                                      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                        <small style={{ color: '#8f8f8f' }}>
                                          {new Date(n.createdAtUtc).toLocaleString()}
                                          {n.updatedAtUtc && <span> · edited {new Date(n.updatedAtUtc).toLocaleString()}</span>}
                                        </small>
                                        <div style={{ display: 'flex', gap: '4px' }}>
                                          {canWrite && <button onClick={() => handleEditNote(n.id, n.content)} style={btn('#374151')}>Edit</button>}
                                          {isAdmin && <button onClick={() => handleDeleteNote(r.id, n.id)} style={btn('#c53030')}>Delete</button>}
                                        </div>
                                      </div>
                                    </div>)}
                                </div>))}
                          {/* add note */}
                          {canWrite && (<div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
                            <input placeholder="Add a note…" value={noteContent} onChange={(e) => setNoteContent(e.target.value)} onKeyDown={(e) => { if (e.key === 'Enter')
                    handleAddNote(r.id); }} style={{ flex: 1, padding: '6px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}/>
                            <button onClick={() => handleAddNote(r.id)} style={{ background: '#2d6a4f', color: '#fff', border: 'none', padding: '6px 14px', cursor: 'pointer', borderRadius: '4px' }}>
                              Add
                            </button>
                          </div>)}
                        </div>
                      </td>
                    </tr>)}
                </Fragment>))}
              {reports.length === 0 && (<tr><td colSpan={7} style={{ padding: '2rem', textAlign: 'center', color: '#8f8f8f' }}>No reports found.</td></tr>)}
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
