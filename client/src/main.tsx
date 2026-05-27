import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { RouterProvider } from 'react-router-dom';
import { MantineProvider } from '@mantine/core';
import { AuthProvider } from './routes/auth/AuthContext';
import createAppRouter from './lib/react-router/routerSetup';
import '@mantine/core/styles.css';
import './App.css';

const router = createAppRouter();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <MantineProvider>
      <AuthProvider>
        <RouterProvider router={router} />
      </AuthProvider>
    </MantineProvider>
  </StrictMode>,
)
