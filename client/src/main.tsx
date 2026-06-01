import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { RouterProvider } from 'react-router'
import { AuthProvider } from './routes/auth/AuthProvider'
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
