import client from './client'
import type { CollectionResponse, ObservationNoteDto, CreateObservationNoteRequest } from '../types/api'

export const notesApi = {
  getByReport: (reportId: string) =>
    client.get<CollectionResponse<ObservationNoteDto>>(`/reports/${reportId}/notes`)
      .then((r) => r.data.items),

  create: (reportId: string, request: CreateObservationNoteRequest) =>
    client.post<ObservationNoteDto>(`/reports/${reportId}/notes`, request).then((r) => r.data),
}
