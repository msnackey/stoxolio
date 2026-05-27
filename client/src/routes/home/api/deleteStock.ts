import apiClient from '../../../lib/axios/apiClient';
import type Stock from "../../../types/stock";

export interface DeleteStockRequest {
    id: number;
}

export interface DeleteStockResponse {
    stock: Stock;
}

export default async function deleteStock(request: DeleteStockRequest): Promise<DeleteStockResponse> {
    try {
        const response = await apiClient.post<DeleteStockResponse>('/stocks/delete', request);
        return response.data;
    } catch (error) {
        return Promise.reject(error);
    }
}