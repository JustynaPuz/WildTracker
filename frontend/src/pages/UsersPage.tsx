import { useEffect, useState } from 'react'
import { usersApi } from '../api/users'
import { ROLE_COLOR } from '../constants'
import type { AppUserDto, UserRole } from '../types/api'

const ROLES: UserRole[] = ['Viewer', 'Ranger', 'Researcher', 'Admin']

export default function UsersPage() {
  const [users, setUsers]       = useState<AppUserDto[]>([])
  const [loading, setLoading]   = useState(true)
  const [error, setError]       = useState<string | null>(null)
  // userId → currently selected role in the dropdown (undefined = not changing)
  const [pendingRole, setPendingRole] = useState<Record<string, UserRole>>({})
  const [saving, setSaving]           = useState<Record<string, boolean>>({})
  const [togglingStatus, setTogglingStatus] = useState<Record<string, boolean>>({})

  const loadUsers = () => {
    setLoading(true)
    usersApi.getAll()
      .then(setUsers)
      .catch(() => setError('Failed to load users.'))
      .finally(() => setLoading(false))
  }

  useEffect(() => { loadUsers() }, [])

  const handleRoleChange = (userId: string, role: UserRole) =>
    setPendingRole((prev) => ({ ...prev, [userId]: role }))

  const handleSaveRole = async (userId: string) => {
    const role = pendingRole[userId]
    if (!role) return
    setSaving((prev) => ({ ...prev, [userId]: true }))
    try {
      await usersApi.changeRole(userId, { role })
      setPendingRole((prev) => { const next = { ...prev }; delete next[userId]; return next })
      loadUsers()
    } catch {
      setError('Failed to change role.')
    } finally {
      setSaving((prev) => ({ ...prev, [userId]: false }))
    }
  }

  const handleToggleStatus = async (userId: string, isActive: boolean) => {
    setTogglingStatus((prev) => ({ ...prev, [userId]: true }))
    try {
      isActive ? await usersApi.deactivate(userId) : await usersApi.activate(userId)
      loadUsers()
    } catch {
      setError('Failed to update user status.')
    } finally {
      setTogglingStatus((prev) => ({ ...prev, [userId]: false }))
    }
  }

  return (
    <div>
      <h1 style={{ color: '#2d6a4f', marginBottom: '1.5rem' }}>Users</h1>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {loading ? <p>Loading…</p> : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ background: '#d8f3dc', textAlign: 'left' }}>
              {['Name', 'Email', 'Role', 'Status', 'Joined', 'Change Role', ''].map((h) => (
                <th key={h} style={{ padding: '8px 12px', borderBottom: '2px solid #2d6a4f' }}>{h}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {users.map((u) => {
              const current  = pendingRole[u.id] ?? u.role
              const isDirty  = pendingRole[u.id] !== undefined && pendingRole[u.id] !== u.role
              const isSaving = saving[u.id] ?? false

              return (
                <tr key={u.id} style={{ borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '8px 12px' }}>{u.firstName} {u.lastName}</td>
                  <td style={{ padding: '8px 12px', color: '#555' }}>{u.email}</td>
                  <td style={{ padding: '8px 12px' }}>
                    <span style={{
                      color: ROLE_COLOR[u.role], fontWeight: 600,
                      background: ROLE_COLOR[u.role] + '18',
                      padding: '2px 8px', borderRadius: '12px', fontSize: '0.85rem',
                    }}>
                      {u.role}
                    </span>
                  </td>
                  <td style={{ padding: '8px 12px' }}>
                    <span style={{ color: u.isActive ? '#276749' : '#c53030', fontWeight: 500 }}>
                      {u.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td style={{ padding: '8px 12px', color: '#555', fontSize: '0.9rem' }}>
                    {new Date(u.createdAtUtc).toLocaleDateString()}
                  </td>
                  <td style={{ padding: '8px 12px' }}>
                    <div style={{ display: 'flex', gap: '6px', alignItems: 'center' }}>
                      <select
                        value={current}
                        onChange={(e) => handleRoleChange(u.id, e.target.value as UserRole)}
                        style={{ padding: '4px 8px', borderRadius: '4px', border: '1px solid #ccc' }}
                      >
                        {ROLES.map((r) => <option key={r} value={r}>{r}</option>)}
                      </select>
                      {isDirty && (
                        <button
                          onClick={() => handleSaveRole(u.id)}
                          disabled={isSaving}
                          style={{
                            background: '#2d6a4f', color: '#fff', border: 'none',
                            padding: '4px 12px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.85rem',
                          }}
                        >
                          {isSaving ? 'Saving…' : 'Save'}
                        </button>
                      )}
                    </div>
                  </td>
                  <td style={{ padding: '8px 12px' }}>
                    <button
                      onClick={() => handleToggleStatus(u.id, u.isActive)}
                      disabled={togglingStatus[u.id]}
                      style={{
                        background: u.isActive ? '#fff' : '#2d6a4f',
                        color: u.isActive ? '#c53030' : '#fff',
                        border: `1px solid ${u.isActive ? '#c53030' : '#2d6a4f'}`,
                        padding: '4px 10px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.85rem',
                        whiteSpace: 'nowrap',
                      }}
                    >
                      {togglingStatus[u.id] ? '…' : u.isActive ? 'Deactivate' : 'Activate'}
                    </button>
                  </td>
                </tr>
              )
            })}
            {users.length === 0 && (
              <tr>
                <td colSpan={7} style={{ padding: '2rem', textAlign: 'center', color: '#666' }}>
                  No users found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  )
}
