import { createBrowserRouter, Navigate, type RouteObject } from 'react-router'
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
  return createBrowserRouter([
    {
      path: '/',
      element: <RootLayout />,
      children: [
        {
          element: <AppShell />,
          hydrateFallbackElement: <AppShellLoader />,
          errorElement: <GlobalErrorPage title="Stoxolio" offSet={32} />,
          children: [
            {
              id: 'home',
              index: true,
              middleware: [authMiddleware],
              loader: categoriesAndStocksLoader,
              shouldRevalidate: () => false,
              element: <HomePage />,
            },
            {
              path: '/login',
              element: <LoginPage />,
            },
            {
              path: '/register',
              element: <RegisterPage />,
            },
          ],
        },
        {
          path: '*',
          element: <Navigate to="/" replace />,
        },
      ],
    },
  ] satisfies AppRouteObject[])
}
