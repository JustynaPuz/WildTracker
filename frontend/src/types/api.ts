// ── Enums ──────────────────────────────────────────────────────────────────────

export type Species =
  | 'Unknown' | 'Wolf' | 'Fox' | 'Bear' | 'Deer'
  | 'Boar' | 'Lynx' | 'Moose' | 'Bird' | 'Other'

export type AnimalHealthStatus = 'Unknown' | 'Healthy' | 'Injured' | 'Sick' | 'Dead'

export type ReportStatus = 'Pending' | 'Verified' | 'Rejected' | 'Resolved'

export type UserRole = 'Viewer' | 'Ranger' | 'Researcher' | 'Admin'

export type ReportType = 'Sighting' | 'Injury' | 'Death' | 'DangerousBehavior' | 'Other'

export type SightingSource = 'Manual' | 'CameraTrap' | 'Drone' | 'Sensor' | 'Imported'

// ── Hypermedia ─────────────────────────────────────────────────────────────────

export interface Link {
  rel: string
  href: string
  method: string
}

// ── Envelope types ─────────────────────────────────────────────────────────────

export interface CollectionResponse<T> {
  items: T[]
  links: Link[]
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
  links: Link[]
}

// ── DTOs (responses) ───────────────────────────────────────────────────────────

export interface AnimalDto {
  id: string
  identifier: string
  name: string
  species: Species
  healthStatus: AnimalHealthStatus
  description?: string
  lastSeenAtUtc?: string
  createdAtUtc: string
  links: Link[]
}

export interface LocationDetailsDto {
  latitude: number
  longitude: number
  region?: string
  forestDistrict?: string
}

export interface SightingReportDto {
  id: string
  animalId: string
  reportedByUserId: string
  observedAtUtc: string
  reportType: ReportType
  status: ReportStatus
  source: SightingSource
  location: LocationDetailsDto
  description?: string
  createdAtUtc: string
  links: Link[]
}

export interface ObservationNoteDto {
  id: string
  sightingReportId: string
  authorUserId: string
  content: string
  createdAtUtc: string
  updatedAtUtc?: string
  links: Link[]
}

export interface AppUserDto {
  id: string
  firstName: string
  lastName: string
  email: string
  role: UserRole
  isActive: boolean
  createdAtUtc: string
  links: Link[]
}

export interface MovementPointDto {
  reportId: string
  observedAtUtc: string
  latitude: number
  longitude: number
  region?: string
  forestDistrict?: string
  reportType: ReportType
  status: ReportStatus
  links: Link[]
}

export interface AuthResponseDto {
  token: string
  expiresAt: string
  user: AppUserDto
}

// ── Requests (inputs) ─────────────────────────────────────────────────────────

export interface CreateAnimalRequest {
  identifier: string
  name: string
  species: Species
  healthStatus: AnimalHealthStatus
  description?: string
}

export interface UpdateAnimalRequest {
  name: string
  species: Species
  healthStatus: AnimalHealthStatus
  description?: string
}

export interface UpdateSightingReportRequest {
  observedAtUtc: string
  reportType: ReportType
  source: SightingSource
  latitude: number
  longitude: number
  region?: string
  forestDistrict?: string
  description?: string
}

export interface CreateSightingReportRequest {
  animalId: string
  observedAtUtc: string
  reportType: ReportType
  source: SightingSource
  latitude: number
  longitude: number
  region?: string
  forestDistrict?: string
  description?: string
}

export interface CreateObservationNoteRequest {
  content: string
}

export interface UpdateObservationNoteRequest {
  content: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  firstName: string
  lastName: string
  email: string
  password: string
}

export interface SightingReportSearchRequest {
  animalId?: string
  reportedByUserId?: string
  status?: ReportStatus
  page?: number
  pageSize?: number
}

export interface UpdateUserRoleRequest {
  role: UserRole
}

// ── Stats ──────────────────────────────────────────────────────────────────────

export interface StatsSummaryDto {
  totalAnimals: number
  totalReports: number
  pendingReports: number
  verifiedReports: number
  rejectedReports: number
  resolvedReports: number
  totalNotes: number
  totalUsers: number
}

export interface SightingsBySpeciesDto {
  species: string
  count: number
}

export interface SightingsByMonthDto {
  year: number
  month: number
  count: number
  label: string
}
