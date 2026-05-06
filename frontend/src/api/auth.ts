import client from './client'
import type { AuthResponseDto, LoginRequest, RegisterRequest } from '../types/api'

export const authApi = {
  login: (request: LoginRequest) =>
    client.post<AuthResponseDto>('/auth/login', request).then((r) => r.data),

  register: (request: RegisterRequest) =>
    client.post<AuthResponseDto>('/auth/register', request).then((r) => r.data),
}
