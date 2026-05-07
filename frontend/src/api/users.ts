import client from './client'
import type { CollectionResponse, AppUserDto, UpdateUserRoleRequest } from '../types/api'

export const usersApi = {
  getAll: () =>
    client.get<CollectionResponse<AppUserDto>>('/users').then((r) => r.data.items),

  getById: (id: string) =>
    client.get<AppUserDto>(`/users/${id}`).then((r) => r.data),

  changeRole: (id: string, request: UpdateUserRoleRequest) =>
    client.put<AppUserDto>(`/users/${id}/role`, request).then((r) => r.data),

  activate: (id: string) =>
    client.post<AppUserDto>(`/users/${id}/activate`).then((r) => r.data),

  deactivate: (id: string) =>
    client.post<AppUserDto>(`/users/${id}/deactivate`).then((r) => r.data),
}
