import type { ReportStatus, UserRole } from './types/api'

export const STATUS_COLOR: Record<ReportStatus, string> = {
  Pending:  '#d97706',
  Verified: '#2d6a4f',
  Rejected: '#c53030',
  Resolved: '#6b7280',
}

export const ROLE_COLOR: Record<UserRole, string> = {
  Viewer:     '#6b7280',
  Ranger:     '#2d6a4f',
  Researcher: '#1d4ed8',
  Admin:      '#c53030',
}
