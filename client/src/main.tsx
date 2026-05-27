import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { RouterProvider } from 'react-router-dom'
import { AuthProvider } from './routes/auth/AuthContext'
import createAppRouter from './lib/react-router/routerSetup'
import '@mantine/core/styles.css'
import ThemeProvider from './lib/mantine/theme'

const router = createAppRouter()

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider>
      <AuthProvider>
        <RouterProvider router={router} />
      </AuthProvider>
    </ThemeProvider>
  </StrictMode>,
)
