import apiClient from '../../../lib/axios/apiClient'
import type AuthResponse from '../types/AuthResponse'

export default async function login(username: string, password: string): Promise<AuthResponse> {
  const response = await apiClient.post<AuthResponse>('/auth/login', {
    username,
    password,
  })
  return response.data
}
