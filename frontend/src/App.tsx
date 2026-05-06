import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Layout from './components/Layout'
import DashboardPage from './pages/DashboardPage'
import AnimalsPage from './pages/AnimalsPage'
import ReportsPage from './pages/ReportsPage'
import MapPage from './pages/MapPage'
import StatsPage from './pages/StatsPage'
import UsersPage from './pages/UsersPage'

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route index element={<Navigate to="/dashboard" replace />} />
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/animals"   element={<AnimalsPage />} />
          <Route path="/reports"   element={<ReportsPage />} />
          <Route path="/map"       element={<MapPage />} />
          <Route path="/stats"     element={<StatsPage />} />
          <Route path="/users"     element={<UsersPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
