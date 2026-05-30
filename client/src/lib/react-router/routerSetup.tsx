import { createBrowserRouter, Navigate, type RouteObject } from 'react-router-dom'
import type RouteHandle from './router.types'
import RootLayout from '../../routes/shell/components/RootLayout'
import categoriesAndStocksLoader from '../../routes/shell/loaders/categoriesAndStocksLoader'
import GlobalErrorPage from '../../routes/shell/components/GlobalErrorPage'
import AppShellLoader from '../../routes/shell/components/AppShellLoader'
import AppShell from '../../routes/shell/components/AppShell'
import LoginPage from '../../routes/auth/LoginPage'
import RegisterPage from '../../routes/auth/RegisterPage'
import HomePage from '../../routes/home/HomePage'
import authMiddleware from '../../routes/auth/authMiddleware'

type AppRouteObject = RouteObject & {
  handle?: RouteHandle
}

export default function createAppRouter() {
  return createBrowserRouter(
    [
      {
        element: <RootLayout />,
        children: [
          {
            path: '/login',
            element: <LoginPage />,
          },
          {
            path: '/register',
            element: <RegisterPage />,
          },
          {
            element: <AppShell />,
            middleware: [authMiddleware],
            loader: categoriesAndStocksLoader,
            id: 'shell',
            path: '/',
            shouldRevalidate: () => false,
            hydrateFallbackElement: <AppShellLoader />,
            errorElement: <GlobalErrorPage title="Stoxolio" offSet={32} />,
            children: [
              {
                index: true,
                element: <HomePage />,
              },
            ],
          },
          {
            path: '*',
            element: <Navigate to="/" replace />,
          },
        ],
      },
    ] satisfies AppRouteObject[],
    { future: { v8_middleware: true } },
  )
}
