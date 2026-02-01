import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import api from '../services/api';
import type { Category } from '../types';

export function DashboardPage() {
    const { username, logout } = useAuth();
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await api.getCategories();
                setCategories(data);
            } catch (err) {
                setError('Failed to load categories');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    return (
        <div style={{ padding: '20px' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <h1>Portfolio Dashboard</h1>
                <div>
                    <span style={{ marginRight: '15px' }}>Welcome, {username}!</span>
                    <button onClick={logout}>Logout</button>
                </div>
            </div>

            {error && <p style={{ color: 'red' }}>{error}</p>}

            {loading ? (
                <p>Loading...</p>
            ) : (
                <div>
                    <h2>Categories</h2>
                    {categories.length === 0 ? (
                        <p>No categories yet. Create one to get started.</p>
                    ) : (
                        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '20px' }}>
                            {categories.map((category) => (
                                <div key={category.id} style={{ border: '1px solid #ddd', padding: '15px', borderRadius: '8px' }}>
                                    <h3>{category.name}</h3>
                                    <p>Target: ${category.target.toFixed(2)}</p>
                                    <p>Stocks: {category.stocks.length}</p>
                                    {category.stocks.length > 0 && (
                                        <div>
                                            <h4>Holdings:</h4>
                                            <ul>
                                                {category.stocks.map((stock) => (
                                                    <li key={stock.id}>
                                                        <strong>{stock.ticker}</strong> ({stock.name})
                                                        <br />
                                                        Shares: {stock.shares} @ ${stock.price.toFixed(2)} = ${stock.value.toFixed(2)}
                                                    </li>
                                                ))}
                                            </ul>
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}
