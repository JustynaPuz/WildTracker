import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { authApi } from '../api/auth'
import { useAuth } from '../context/AuthContext'

export default function RegisterPage() {
  const { setSession } = useAuth()
  const navigate = useNavigate()
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '', confirm: '' })
  const [error, setError]   = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (form.password !== form.confirm) {
      setError('Passwords do not match.')
      return
    }
    setError(null)
    setLoading(true)
    try {
      const response = await authApi.register({
        firstName: form.firstName,
        lastName:  form.lastName,
        email:     form.email,
        password:  form.password,
      })
      setSession(response)
      navigate('/dashboard', { replace: true })
    } catch {
      setError('Registration failed. The email may already be in use.')
    } finally {
      setLoading(false)
    }
  }

  const field: React.CSSProperties = {
    display: 'block', width: '100%', padding: '8px 10px', marginTop: '4px',
    borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box', fontSize: '0.95rem',
  }

  return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh', background: '#f0f7f0' }}>
      <form onSubmit={handleSubmit} style={{ background: '#fff', padding: '2rem', borderRadius: '8px', boxShadow: '0 2px 8px rgba(0,0,0,0.12)', width: '360px' }}>
        <h1 style={{ color: '#2d6a4f', marginTop: 0, marginBottom: '0.25rem', textAlign: 'center', fontSize: '1.6rem' }}>WildTracker</h1>
        <p style={{ color: '#555', textAlign: 'center', marginTop: 0, marginBottom: '1.5rem', fontSize: '0.9rem' }}>Create an account</p>

        {error && (
          <p style={{ color: '#c53030', background: '#fff5f5', padding: '8px 12px', borderRadius: '4px', marginBottom: '1rem', fontSize: '0.9rem' }}>
            {error}
          </p>
        )}

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '0.75rem', marginBottom: '0.75rem' }}>
          <label style={{ fontSize: '0.9rem' }}>
            First Name
            <input value={form.firstName} onChange={e => setForm(f => ({ ...f, firstName: e.target.value }))}
              required autoFocus style={field} />
          </label>
          <label style={{ fontSize: '0.9rem' }}>
            Last Name
            <input value={form.lastName} onChange={e => setForm(f => ({ ...f, lastName: e.target.value }))}
              required style={field} />
          </label>
        </div>

        <label style={{ display: 'block', marginBottom: '0.75rem', fontSize: '0.9rem' }}>
          Email
          <input type="email" value={form.email} onChange={e => setForm(f => ({ ...f, email: e.target.value }))}
            required style={field} />
        </label>

        <label style={{ display: 'block', marginBottom: '0.75rem', fontSize: '0.9rem' }}>
          Password
          <input type="password" value={form.password} onChange={e => setForm(f => ({ ...f, password: e.target.value }))}
            required minLength={8} style={field} />
        </label>

        <label style={{ display: 'block', marginBottom: '1.5rem', fontSize: '0.9rem' }}>
          Confirm Password
          <input type="password" value={form.confirm} onChange={e => setForm(f => ({ ...f, confirm: e.target.value }))}
            required style={field} />
        </label>

        <button
          type="submit"
          disabled={loading}
          style={{ width: '100%', padding: '10px', background: loading ? '#74b49b' : '#2d6a4f', color: '#fff', border: 'none', borderRadius: '4px', cursor: loading ? 'not-allowed' : 'pointer', fontSize: '1rem' }}
        >
          {loading ? 'Creating account…' : 'Create account'}
        </button>

        <p style={{ textAlign: 'center', marginTop: '1.25rem', marginBottom: 0, fontSize: '0.9rem', color: '#555' }}>
          Already have an account?{' '}
          <Link to="/login" style={{ color: '#2d6a4f' }}>Sign in</Link>
        </p>
      </form>
    </div>
  )
}
