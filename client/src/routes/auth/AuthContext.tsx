import React, { createContext, useState, useContext, useCallback, useEffect } from 'react';
import { setUnauthorizedHandler } from '../../lib/axios/apiClient';
import loginApi from './api/login';
import logoutApi from './api/logout';
import registerApi from './api/register';

interface AuthContextType {
    isAuthenticated: boolean;
    username: string | null;
    login: (username: string, password: string) => Promise<boolean>;
    register: (username: string, email: string, password: string) => Promise<boolean>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [username, setUsername] = useState<string | null>(() => {
        return localStorage.getItem('username');
    });

    const isAuthenticated = username !== null;

    const clearSession = useCallback(() => {
        localStorage.removeItem('username');
        setUsername(null);
    }, []);

    useEffect(() => {
        setUnauthorizedHandler(clearSession);
    }, [clearSession]);

    const login = useCallback(async (username: string, password: string) => {
        try {
            const response = await loginApi(username, password);
            if (response.success) {
                localStorage.setItem('username', username);
                setUsername(username);
                return true;
            }
            return false;
        } catch {
            return false;
        }
    }, []);

    const register = useCallback(async (username: string, email: string, password: string) => {
        try {
            const response = await registerApi(username, email, password);
            if (response.success && response.username) {
                localStorage.setItem('username', response.username);
                setUsername(response.username);
                return true;
            }
            return false;
        } catch {
            return false;
        }
    }, []);

    const logout = useCallback(async () => {
        try {
            await logoutApi();
        } finally {
            clearSession();
        }
    }, [clearSession]);

    return (
        <AuthContext.Provider value={{ isAuthenticated, username, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within AuthProvider');
    }
    return context;
}
