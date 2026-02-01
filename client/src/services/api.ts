import axios, { type AxiosInstance } from 'axios';
import type { Category, Stock, AuthResponse } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

class ApiClient {
    private client: AxiosInstance;

    constructor() {
        this.client = axios.create({
            baseURL: API_BASE_URL,
            headers: {
                'Content-Type': 'application/json',
            },
        });

        // Add JWT token to headers if available
        this.client.interceptors.request.use((config) => {
            const token = localStorage.getItem('auth_token');
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        });
    }

    // Auth endpoints
    async register(username: string, email: string, password: string): Promise<AuthResponse> {
        const response = await this.client.post<AuthResponse>('/auth/register', {
            username,
            email,
            password,
        });
        return response.data;
    }

    async login(username: string, password: string): Promise<AuthResponse> {
        const response = await this.client.post<AuthResponse>('/auth/login', {
            username,
            password,
        });
        return response.data;
    }

    // Category endpoints
    async getCategories(): Promise<Category[]> {
        const response = await this.client.get<Category[]>('/categories');
        return response.data;
    }

    async getCategory(id: number): Promise<Category> {
        const response = await this.client.get<Category>(`/categories/${id}`);
        return response.data;
    }

    async createCategory(name: string, target: number): Promise<Category> {
        const response = await this.client.post<Category>('/categories', { name, target });
        return response.data;
    }

    async updateCategory(id: number, name: string, target: number): Promise<void> {
        await this.client.put(`/categories/${id}`, { name, target });
    }

    async deleteCategory(id: number): Promise<void> {
        await this.client.delete(`/categories/${id}`);
    }

    // Stock endpoints
    async getStocks(): Promise<Stock[]> {
        const response = await this.client.get<Stock[]>('/stocks');
        return response.data;
    }

    async getStock(id: number): Promise<Stock> {
        const response = await this.client.get<Stock>(`/stocks/${id}`);
        return response.data;
    }

    async createStock(stock: Omit<Stock, 'id' | 'value' | 'priceChange' | 'valueChange'>): Promise<Stock> {
        const response = await this.client.post<Stock>('/stocks', stock);
        return response.data;
    }

    async updateStock(id: number, stock: Omit<Stock, 'id' | 'value' | 'priceChange' | 'valueChange'>): Promise<void> {
        await this.client.put(`/stocks/${id}`, stock);
    }

    async deleteStock(id: number): Promise<void> {
        await this.client.delete(`/stocks/${id}`);
    }
}

export default new ApiClient();
