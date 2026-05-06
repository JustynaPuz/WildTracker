import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [email, setEmail]       = useState('')
  const [password, setPassword] = useState('')
  const [error, setError]       = useState<string | null>(null)
  const [loading, setLoading]   = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setLoading(true)
    try {
      await login({ email, password })
      navigate('/animals', { replace: true })
    } catch {
      setError('Invalid email or password.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh', background: '#f0f7f0' }}>
      <form onSubmit={handleSubmit} style={{ background: '#fff', padding: '2rem', borderRadius: '8px', boxShadow: '0 2px 8px rgba(0,0,0,0.12)', width: '320px' }}>
        <h1 style={{ color: '#2d6a4f', marginTop: 0, marginBottom: '0.25rem', textAlign: 'center', fontSize: '1.6rem' }}>WildTracker</h1>
        <p style={{ color: '#555', textAlign: 'center', marginTop: 0, marginBottom: '1.5rem', fontSize: '0.9rem' }}>Sign in to continue</p>

        {error && (
          <p style={{ color: '#c53030', background: '#fff5f5', padding: '8px 12px', borderRadius: '4px', marginBottom: '1rem', fontSize: '0.9rem' }}>
            {error}
          </p>
        )}

        <label style={{ display: 'block', marginBottom: '1rem', fontSize: '0.9rem' }}>
          Email
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            autoFocus
            placeholder="ranger@wildtracker.pl"
            style={{ display: 'block', width: '100%', padding: '8px 10px', marginTop: '4px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box', fontSize: '0.95rem' }}
          />
        </label>

        <label style={{ display: 'block', marginBottom: '1.5rem', fontSize: '0.9rem' }}>
          Password
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            style={{ display: 'block', width: '100%', padding: '8px 10px', marginTop: '4px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box', fontSize: '0.95rem' }}
          />
        </label>

        <button
          type="submit"
          disabled={loading}
          style={{ width: '100%', padding: '10px', background: loading ? '#74b49b' : '#2d6a4f', color: '#fff', border: 'none', borderRadius: '4px', cursor: loading ? 'not-allowed' : 'pointer', fontSize: '1rem' }}
        >
          {loading ? 'Signing in…' : 'Sign in'}
        </button>

        <p style={{ color: '#888', fontSize: '0.8rem', textAlign: 'center', marginBottom: 0, marginTop: '1.5rem' }}>
          Demo: ranger@wildtracker.pl / Ranger123!
        </p>
      </form>
    </div>
  )
}
