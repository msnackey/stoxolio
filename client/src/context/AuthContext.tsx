import React, { createContext, useState, useContext, useCallback } from 'react';
import api from '../services/api';

interface AuthContextType {
    isAuthenticated: boolean;
    username: string | null;
    login: (username: string, password: string) => Promise<boolean>;
    register: (username: string, email: string, password: string) => Promise<boolean>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState(() => {
        return localStorage.getItem('auth_token') !== null;
    });
    const [username, setUsername] = useState<string | null>(() => {
        return localStorage.getItem('username');
    });

    const login = useCallback(async (username: string, password: string) => {
        try {
            const response = await api.login(username, password);
            if (response.success && response.token) {
                localStorage.setItem('auth_token', response.token);
                localStorage.setItem('username', username);
                setIsAuthenticated(true);
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
            const response = await api.register(username, email, password);
            if (response.success && response.token) {
                localStorage.setItem('auth_token', response.token);
                localStorage.setItem('username', username);
                setIsAuthenticated(true);
                setUsername(username);
                return true;
            }
            return false;
        } catch {
            return false;
        }
    }, []);

    const logout = useCallback(() => {
        localStorage.removeItem('auth_token');
        localStorage.removeItem('username');
        setIsAuthenticated(false);
        setUsername(null);
    }, []);

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
