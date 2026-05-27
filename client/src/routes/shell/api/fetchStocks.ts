import apiClient from '../../../lib/axios/apiClient'
import type Stock from '../../../types/stock'

export interface GetStocksResponse {
  stocks: Stock[]
}

export default async function fetchStocks(): Promise<GetStocksResponse> {
  try {
    const response = await apiClient.get<GetStocksResponse>('/stocks')
    return response.data
  } catch (error) {
    return Promise.reject(error)
  }
}
