import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
export default function LoginPage() {
    const { login } = useAuth();
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setLoading(true);
        try {
            await login({ email, password });
            navigate('/dashboard', { replace: true });
        }
        catch {
            setError('Invalid email or password.');
        }
        finally {
            setLoading(false);
        }
    };
    return (<div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh', background: '#121212' }}>
      <form onSubmit={handleSubmit} style={{ background: '#1e1e1e', padding: '2rem', borderRadius: '8px', boxShadow: '0 8px 24px rgba(0,0,0,0.5)', border: '1px solid #2a2a2a', width: '320px' }}>
        <h1 style={{ color: '#34d399', marginTop: 0, marginBottom: '0.25rem', textAlign: 'center', fontSize: '1.6rem' }}>WildTracker</h1>
        <p style={{ color: '#b3b3b3', textAlign: 'center', marginTop: 0, marginBottom: '1.5rem', fontSize: '0.9rem' }}>Sign in to continue</p>

        {error && (<p style={{ color: '#f87171', background: 'rgba(229,72,77,0.12)', padding: '8px 12px', borderRadius: '4px', marginBottom: '1rem', fontSize: '0.9rem' }}>
            {error}
          </p>)}

        <label style={{ display: 'block', marginBottom: '1rem', fontSize: '0.9rem', color: '#d4d4d4' }}>
          Email
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required autoFocus placeholder="ranger@wildtracker.pl" style={{ display: 'block', width: '100%', padding: '8px 10px', marginTop: '4px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8', boxSizing: 'border-box', fontSize: '0.95rem' }}/>
        </label>

        <label style={{ display: 'block', marginBottom: '1.5rem', fontSize: '0.9rem', color: '#d4d4d4' }}>
          Password
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required style={{ display: 'block', width: '100%', padding: '8px 10px', marginTop: '4px', borderRadius: '4px', border: '1px solid #3a3a3a', background: '#141414', color: '#e8e8e8', boxSizing: 'border-box', fontSize: '0.95rem' }}/>
        </label>

        <button type="submit" disabled={loading} style={{ width: '100%', padding: '10px', background: loading ? '#2f5d47' : '#2d6a4f', color: '#fff', border: 'none', borderRadius: '4px', cursor: loading ? 'not-allowed' : 'pointer', fontSize: '1rem' }}>
          {loading ? 'Signing in…' : 'Sign in'}
        </button>

        <p style={{ textAlign: 'center', marginTop: '1.25rem', marginBottom: 0, fontSize: '0.9rem', color: '#b3b3b3' }}>
          No account?{' '}
          <Link to="/register" style={{ color: '#34d399' }}>Create one</Link>
        </p>

        <div style={{ color: '#8f8f8f', fontSize: '0.8rem', textAlign: 'center', marginBottom: 0, marginTop: '0.75rem', lineHeight: 1.6 }}>
          <div>Demo — Viewer: viewer@wildtracker.pl / Viewer123!</div>
          <div>Demo — Ranger: ranger@wildtracker.pl / Ranger123!</div>
          <div>Demo — Admin: admin@wildtracker.pl / Admin123!</div>
        </div>
      </form>
    </div>);
}
