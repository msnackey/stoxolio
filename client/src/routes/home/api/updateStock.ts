import apiClient from '../../../lib/axios/apiClient'
import type Stock from '../../../types/stock'

export interface UpdateStockRequest {
  stock: Stock
}

export interface UpdateStockResponse {
  stock: Stock
}

export default async function updateStock(
  request: UpdateStockRequest,
): Promise<UpdateStockResponse> {
  try {
    const response = await apiClient.put<UpdateStockResponse>('/stocks', request)
    return response.data
  } catch (error) {
    return Promise.reject(error)
  }
}
