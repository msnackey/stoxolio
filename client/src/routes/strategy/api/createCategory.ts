import apiClient from '../../../lib/axios/apiClient'
import type Category from '../../../types/category'

export interface CreateCategoryRequest {
  category: Category
}

export interface CreateCategoryResponse {
  category: Category
}

export default async function createCategory(
  request: CreateCategoryRequest,
): Promise<CreateCategoryResponse> {
  try {
    const response = await apiClient.post<CreateCategoryResponse>('/categories', request)
    return response.data
  } catch (error) {
    return Promise.reject(error)
  }
}
