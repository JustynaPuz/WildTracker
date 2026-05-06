import client from './client'
import type {
  SightingReportDto,
  PagedResult,
  CreateSightingReportRequest,
  SightingReportSearchRequest,
} from '../types/api'

export const reportsApi = {
  search: (params: SightingReportSearchRequest) =>
    client.get<PagedResult<SightingReportDto>>('/reports', { params }).then((r) => r.data),

  getById: (id: string) =>
    client.get<SightingReportDto>(`/reports/${id}`).then((r) => r.data),

  create: (request: CreateSightingReportRequest) =>
    client.post<SightingReportDto>('/reports', request).then((r) => r.data),

  approve: (id: string) =>
    client.post(`/reports/${id}/approve`),

  reject: (id: string) =>
    client.post(`/reports/${id}/reject`),

  resolve: (id: string) =>
    client.post<SightingReportDto>(`/reports/${id}/resolve`).then((r) => r.data),

  delete: (id: string) =>
    client.delete(`/reports/${id}`),
}
