import apiClient from '../../../lib/axios/apiClient';
import type Category from "../../../types/category";

export interface UpdateCategoryRequest {
    category: Category;
}

export interface UpdateCategoryResponse {
    category: Category;
}

export default async function updateCategory(request: UpdateCategoryRequest): Promise<UpdateCategoryResponse> {
    try {
        const response = await apiClient.put<UpdateCategoryResponse>('/categories', request);
        return response.data;
    } catch (error) {
        return Promise.reject(error);
    }
}