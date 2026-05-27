import apiClient from '../../../lib/axios/apiClient';
import type Category from "../../../types/category";

export interface DeleteCategoryRequest {
    id: number;
}

export interface DeleteCategoryResponse {
    category: Category;
}

export default async function deleteCategory(request: DeleteCategoryRequest): Promise<DeleteCategoryResponse> {
    try {
        const response = await apiClient.post<DeleteCategoryResponse>('/categories/delete', request);
        return response.data;
    } catch (error) {
        return Promise.reject(error);
    }
}