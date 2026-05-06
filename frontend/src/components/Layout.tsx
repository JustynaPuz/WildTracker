import { Link, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Layout() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <div style={{ fontFamily: 'sans-serif', minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <header style={{ background: '#2d6a4f', color: '#fff', padding: '0 1.5rem', display: 'flex', alignItems: 'center', gap: '2rem', height: '56px' }}>
        <Link to="/" style={{ color: '#fff', textDecoration: 'none', fontWeight: 700, fontSize: '1.2rem' }}>
          WildTracker
        </Link>
        <nav style={{ display: 'flex', gap: '1.5rem', flex: 1 }}>
          <Link to="/dashboard" style={{ color: '#d8f3dc', textDecoration: 'none' }}>Dashboard</Link>
          <Link to="/animals"   style={{ color: '#d8f3dc', textDecoration: 'none' }}>Animals</Link>
          <Link to="/reports"   style={{ color: '#d8f3dc', textDecoration: 'none' }}>Reports</Link>
          <Link to="/map"       style={{ color: '#d8f3dc', textDecoration: 'none' }}>Map</Link>
          <Link to="/stats"     style={{ color: '#d8f3dc', textDecoration: 'none' }}>Statistics</Link>
          <Link to="/users"     style={{ color: '#d8f3dc', textDecoration: 'none' }}>Users</Link>
        </nav>
        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <span style={{ color: '#b7e4c7', fontSize: '0.85rem' }}>
            {user?.firstName} {user?.lastName}
            <span style={{ color: '#74b49b', marginLeft: '6px' }}>({user?.role})</span>
          </span>
          <button
            onClick={handleLogout}
            style={{ background: 'transparent', border: '1px solid #74b49b', color: '#d8f3dc', padding: '4px 12px', cursor: 'pointer', borderRadius: '4px', fontSize: '0.85rem' }}
          >
            Sign out
          </button>
        </div>
      </header>
      <main style={{ flex: 1, padding: '1.5rem', maxWidth: '1200px', margin: '0 auto', width: '100%', boxSizing: 'border-box' }}>
        <Outlet />
      </main>
    </div>
  )
}
