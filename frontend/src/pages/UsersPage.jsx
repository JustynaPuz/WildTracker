import { useEffect, useState } from 'react';
import { usersApi } from '../api/users';
import { ROLE_COLOR } from '../constants';
const ROLES = ['Viewer', 'Ranger', 'Admin'];
export default function UsersPage() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    // userId → currently selected role in the dropdown (undefined = not changing)
    const [pendingRole, setPendingRole] = useState({});
    const [saving, setSaving] = useState({});
    const [togglingStatus, setTogglingStatus] = useState({});
    const [page, setPage] = useState(1);
    const PAGE_SIZE = 12;
    const loadUsers = () => {
        setLoading(true);
        usersApi.getAll()
            .then(setUsers)
            .catch(() => setError('Failed to load users.'))
            .finally(() => setLoading(false));
    };
    useEffect(() => { loadUsers(); }, []);
    const handleRoleChange = (userId, role) => setPendingRole((prev) => ({ ...prev, [userId]: role }));
    const handleSaveRole = async (userId) => {
        const role = pendingRole[userId];
        if (!role)
            return;
        setSaving((prev) => ({ ...prev, [userId]: true }));
        try {
            await usersApi.changeRole(userId, { role });
            setPendingRole((prev) => { const next = { ...prev }; delete next[userId]; return next; });
            loadUsers();
        }
        catch {
            setError('Failed to change role.');
        }
        finally {
            setSaving((prev) => ({ ...prev, [userId]: false }));
        }
    };
    const handleToggleStatus = async (userId, isActive) => {
        setTogglingStatus((prev) => ({ ...prev, [userId]: true }));
        try {
            isActive ? await usersApi.deactivate(userId) : await usersApi.activate(userId);
            loadUsers();
        }
        catch {
            setError('Failed to update user status.');
        }
        finally {
            setTogglingStatus((prev) => ({ ...prev, [userId]: false }));
        }
    };
    const totalPages = Math.max(1, Math.ceil(users.length / PAGE_SIZE));
    const pagedUsers = users.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
    return (<div>
      <h1 style={{ color: '#34d399', marginBottom: '1.5rem' }}>Users</h1>

      {error && <p style={{ color: '#f87171' }}>{error}</p>}

      {loading ? <p>Loading…</p> : (<>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ background: '#1f3d2f', textAlign: 'left' }}>
              {['Name', 'Email', 'Role', 'Status', 'Joined', 'Change Role', ''].map((h) => (<th key={h} style={{ padding: '8px 12px', borderBottom: '2px solid #2d6a4f' }}>{h}</th>))}
            </tr>
          </thead>
          <tbody>
            {pagedUsers.map((u) => {
                const current = pendingRole[u.id] ?? u.role;
                const isDirty = pendingRole[u.id] !== undefined && pendingRole[u.id] !== u.role;
                const isSaving = saving[u.id] ?? false;
                return (<tr key={u.id} style={{ borderBottom: '1px solid #2a2a2a' }}>
                  <td style={{ padding: '8px 12px' }}>{u.firstName} {u.lastName}</td>
                  <td style={{ padding: '8px 12px', color: '#b3b3b3' }}>{u.email}</td>
                  <td style={{ padding: '8px 12px' }}>
                    <span style={{
                        color: ROLE_COLOR[u.role], fontWeight: 600,
                        background: ROLE_COLOR[u.role] + '26',
                        padding: '2px 8px', borderRadius: '12px', fontSize: '0.85rem',
                    }}>
                      {u.role}
                    </span>
                  </td>
                  <td style={{ padding: '8px 12px' }}>
                    <span style={{ color: u.isActive ? '#3ba776' : '#f87171', fontWeight: 500 }}>
                      {u.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td style={{ padding: '8px 12px', color: '#b3b3b3', fontSize: '0.9rem' }}>
                    {new Date(u.createdAtUtc).toLocaleDateString()}
                  </td>
                  <td style={{ padding: '8px 12px' }}>
                    <div style={{ display: 'flex', gap: '6px', alignItems: 'center' }}>
                      <select value={current} onChange={(e) => handleRoleChange(u.id, e.target.value)} style={{ padding: '4px 8px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8' }}>
                        {ROLES.map((r) => <option key={r} value={r}>{r}</option>)}
                      </select>
                      {isDirty && (<button onClick={() => handleSaveRole(u.id)} disabled={isSaving} style={{
                            background: '#2d6a4f', color: '#fff', border: 'none',
                            padding: '4px 12px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.85rem',
                        }}>
                          {isSaving ? 'Saving…' : 'Save'}
                        </button>)}
                    </div>
                  </td>
                  <td style={{ padding: '8px 12px' }}>
                    <button onClick={() => handleToggleStatus(u.id, u.isActive)} disabled={togglingStatus[u.id]} style={{
                        background: u.isActive ? 'transparent' : '#2d6a4f',
                        color: u.isActive ? '#f87171' : '#fff',
                        border: `1px solid ${u.isActive ? '#f87171' : '#2d6a4f'}`,
                        padding: '4px 10px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.85rem',
                        whiteSpace: 'nowrap',
                    }}>
                      {togglingStatus[u.id] ? '…' : u.isActive ? 'Deactivate' : 'Activate'}
                    </button>
                  </td>
                </tr>);
            })}
            {users.length === 0 && (<tr>
                <td colSpan={7} style={{ padding: '2rem', textAlign: 'center', color: '#8f8f8f' }}>
                  No users found.
                </td>
              </tr>)}
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
