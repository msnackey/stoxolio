import { useNavigate } from 'react-router'
import { useAuth } from '../auth/authContext'
import useCategoriesAndStocksData from '../shell/hooks/useCategoriesAndStocksData'

export default function HomePage() {
  const navigate = useNavigate()
  const { username, logout } = useAuth()
  const { categories } = useCategoriesAndStocksData()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <div style={{ padding: '20px' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1>Portfolio Dashboard</h1>
        <div>
          <span style={{ marginRight: '15px' }}>Welcome, {username}!</span>
          <button onClick={handleLogout}>Logout</button>
        </div>
      </div>

      <div>
        <h2>Categories</h2>
        {categories.length === 0 ? (
          <p>No categories yet. Create one to get started.</p>
        ) : (
          <div
            style={{
              display: 'grid',
              gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
              gap: '20px',
            }}
          >
            {categories.map((category) => (
              <div
                key={category.id}
                style={{ border: '1px solid #ddd', padding: '15px', borderRadius: '8px' }}
              >
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
                          Shares: {stock.shares} @ ${stock.price.toFixed(2)} = $
                          {stock.value.toFixed(2)}
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
    </div>
  )
}
