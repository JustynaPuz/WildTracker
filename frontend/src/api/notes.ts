import client from './client'
import type { ObservationNoteDto, CreateObservationNoteRequest } from '../types/api'

export const notesApi = {
  getByReport: (reportId: string) =>
    client.get<ObservationNoteDto[]>(`/notes/report/${reportId}`).then((r) => r.data),

  create: (request: CreateObservationNoteRequest) =>
    client.post<ObservationNoteDto>('/notes', request).then((r) => r.data),
}
