import axios from 'axios';

const DEFAULT_TIMEOUT = 300_000;

let onUnauthorized: (() => void) | undefined;
let isRefreshing = false;
let refreshQueue: Array<(success: boolean) => void> = [];

function drainQueue(success: boolean) {
    refreshQueue.forEach((cb) => cb(success));
    refreshQueue = [];
}

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
    timeout: DEFAULT_TIMEOUT,
    timeoutErrorMessage: 'Server took too long to response. Please try again later.',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

apiClient.interceptors.request.use((config) => {
    if (import.meta.env.DEV) {
        const token = import.meta.env.VITE_DEV_AUTH_TOKEN;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
    }
    return config;
});

apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;
        const isAuthEndpoint = originalRequest?.url?.includes('/auth/');

        if (error.response?.status === 401 && !originalRequest?._isRetry && !isAuthEndpoint) {
            originalRequest._isRetry = true;

            if (isRefreshing) {
                return new Promise((resolve, reject) => {
                    refreshQueue.push((success) => {
                        success ? resolve(apiClient(originalRequest)) : reject(error);
                    });
                });
            }

            isRefreshing = true;
            try {
                await apiClient.post('/auth/refresh');
                drainQueue(true);
                return apiClient(originalRequest);
            } catch {
                drainQueue(false);
                onUnauthorized?.();
                return Promise.reject(error);
            } finally {
                isRefreshing = false;
            }
        }

        if (error.response?.status === 401 && originalRequest?._isRetry) {
            onUnauthorized?.();
        }

        return Promise.reject(error);
    }
);

export function setUnauthorizedHandler(handler: () => void) {
    onUnauthorized = handler;
}

export default apiClient;