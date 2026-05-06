import { Link, Outlet } from 'react-router-dom'

export default function Layout() {
  return (
    <div style={{ fontFamily: 'sans-serif', minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <header style={{ background: '#2d6a4f', color: '#fff', padding: '0 1.5rem', display: 'flex', alignItems: 'center', gap: '2rem', height: '56px' }}>
        <Link to="/" style={{ color: '#fff', textDecoration: 'none', fontWeight: 700, fontSize: '1.2rem' }}>
          WildTracker
        </Link>
        <nav style={{ display: 'flex', gap: '1.5rem' }}>
          <Link to="/dashboard" style={{ color: '#d8f3dc', textDecoration: 'none' }}>Dashboard</Link>
          <Link to="/animals"  style={{ color: '#d8f3dc', textDecoration: 'none' }}>Animals</Link>
          <Link to="/reports"  style={{ color: '#d8f3dc', textDecoration: 'none' }}>Reports</Link>
          <Link to="/map"      style={{ color: '#d8f3dc', textDecoration: 'none' }}>Map</Link>
          <Link to="/stats"    style={{ color: '#d8f3dc', textDecoration: 'none' }}>Statistics</Link>
          <Link to="/users"    style={{ color: '#d8f3dc', textDecoration: 'none' }}>Users</Link>
        </nav>
      </header>
      <main style={{ flex: 1, padding: '1.5rem', maxWidth: '1200px', margin: '0 auto', width: '100%', boxSizing: 'border-box' }}>
        <Outlet />
      </main>
    </div>
  )
}
