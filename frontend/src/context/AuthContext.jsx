import { createContext, useContext, useState, useCallback } from 'react';
import { authApi } from '../api/auth';
const AuthContext = createContext(null);
const TOKEN_KEY = 'wt_token';
const USER_KEY = 'wt_user';
export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => {
        try {
            const stored = localStorage.getItem(USER_KEY);
            return stored ? JSON.parse(stored) : null;
        }
        catch {
            return null;
        }
    });
    const setSession = useCallback((response) => {
        localStorage.setItem(TOKEN_KEY, response.token);
        localStorage.setItem(USER_KEY, JSON.stringify(response.user));
        setUser(response.user);
    }, []);
    const login = useCallback(async (request) => {
        const response = await authApi.login(request);
        setSession(response);
    }, [setSession]);
    const logout = useCallback(() => {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        setUser(null);
    }, []);
    return (<AuthContext.Provider value={{ user, isAuthenticated: user !== null, login, setSession, logout }}>
      {children}
    </AuthContext.Provider>);
}
export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx)
        throw new Error('useAuth must be used within AuthProvider');
    return ctx;
}
