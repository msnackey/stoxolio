import apiClient from '../../../lib/axios/apiClient'
import type Stock from '../../../types/stock'

export interface CreateStockRequest {
  stock: Stock
}

export interface CreateStockResponse {
  stock: Stock
}

export default async function createStock(
  request: CreateStockRequest,
): Promise<CreateStockResponse> {
  try {
    const response = await apiClient.post<CreateStockResponse>('/stocks', request)
    return response.data
  } catch (error) {
    return Promise.reject(error)
  }
}
