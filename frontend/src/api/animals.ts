import client from './client'
import type { AnimalDto, CreateAnimalRequest, UpdateAnimalRequest } from '../types/api'

export const animalsApi = {
  getAll: () =>
    client.get<AnimalDto[]>('/animals').then((r) => r.data),

  getById: (id: string) =>
    client.get<AnimalDto>(`/animals/${id}`).then((r) => r.data),

  create: (request: CreateAnimalRequest) =>
    client.post<AnimalDto>('/animals', request).then((r) => r.data),

  update: (id: string, request: UpdateAnimalRequest) =>
    client.put<AnimalDto>(`/animals/${id}`, request).then((r) => r.data),

  delete: (id: string) =>
    client.delete(`/animals/${id}`),
}
