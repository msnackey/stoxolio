import apiClient from '../../../lib/axios/apiClient';
import type AuthResponse from "../types/AuthResponse";

export default async function register(username: string, email: string, password: string): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse>('/auth/register', {
        username,
        email,
        password,
    });
    return response.data;
}