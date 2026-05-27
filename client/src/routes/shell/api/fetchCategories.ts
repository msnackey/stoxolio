import apiClient from '../../../lib/axios/apiClient'
import type Category from '../../../types/category'

export interface GetCategoriesResponse {
  categories: Category[]
}

export default async function fetchCategories(): Promise<GetCategoriesResponse> {
  try {
    const response = await apiClient.get<GetCategoriesResponse>('/categories')
    return response.data
  } catch (error) {
    return Promise.reject(error)
  }
}
