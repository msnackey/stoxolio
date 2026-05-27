import apiClient from '../../../lib/axios/apiClient'

export default async function logout(): Promise<void> {
  await apiClient.post('/auth/logout')
}
