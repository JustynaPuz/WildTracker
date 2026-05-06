import client from './client'
import type { CollectionResponse, ObservationNoteDto, CreateObservationNoteRequest, UpdateObservationNoteRequest } from '../types/api'

export const notesApi = {
  getByReport: (reportId: string) =>
    client.get<CollectionResponse<ObservationNoteDto>>(`/reports/${reportId}/notes`)
      .then((r) => r.data.items),

  getById: (reportId: string, noteId: string) =>
    client.get<ObservationNoteDto>(`/reports/${reportId}/notes/${noteId}`).then((r) => r.data),

  create: (reportId: string, request: CreateObservationNoteRequest) =>
    client.post<ObservationNoteDto>(`/reports/${reportId}/notes`, request).then((r) => r.data),

  update: (reportId: string, noteId: string, request: UpdateObservationNoteRequest) =>
    client.put<ObservationNoteDto>(`/reports/${reportId}/notes/${noteId}`, request).then((r) => r.data),

  delete: (reportId: string, noteId: string) =>
    client.delete(`/reports/${reportId}/notes/${noteId}`),
}
