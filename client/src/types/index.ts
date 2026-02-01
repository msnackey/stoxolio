export interface Stock {
    id: number;
    name: string;
    ticker: string;
    exchange: string;
    sri: boolean;
    shares: number;
    price: number;
    invest: boolean;
    categoryId: number;
    prevPrice: number;
    value: number;
    priceChange: number;
    valueChange: number;
}

export interface Category {
    id: number;
    name: string;
    target: number;
    stocks: Stock[];
}

export interface User {
    id: number;
    username: string;
    email: string;
}

export interface AuthResponse {
    success: boolean;
    message: string;
    token?: string;
}
