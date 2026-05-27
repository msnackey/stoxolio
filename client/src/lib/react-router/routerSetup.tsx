import { createBrowserRouter, Navigate, type RouteObject } from "react-router-dom";
import type RouteHandle from "./router.types";
import RootLayout from "../../routes/shell/components/RootLayout";
import categoriesAndStocksLoader from "../../routes/shell/loaders/categoriesAndStocksLoader";
import GlobalErrorPage from "../../routes/shell/components/GlobalErrorPage";
import AppShellLoader from "../../routes/shell/components/AppShellLoader";
import AppShell from "../../routes/shell/components/AppShell";
import LoginPage from "../../routes/auth/LoginPage";
import RegisterPage from "../../routes/auth/RegisterPage";
import ProtectedRoute from "../../routes/auth/components/ProtectedRoute";
import HomePage from "../../routes/home/HomePage";

type AppRouteObject = RouteObject & {
    handle?: RouteHandle;
}

export default function createAppRouter() {
    return createBrowserRouter(
        [
            {
                element: <RootLayout />,
                children: [
                    {
                        path: "/login",
                        element: <LoginPage />
                    },
                    {
                        path: "/register",
                        element: <RegisterPage />
                    },
                    {
                        element: <AppShell />,
                        loader: categoriesAndStocksLoader,
                        id: "root",
                        path: "/",
                        shouldRevalidate: () => false,
                        hydrateFallbackElement: <AppShellLoader />,
                        errorElement: <GlobalErrorPage title="" offSet={32} />,
                        children: [
                            {
                                index: true,
                                element: <ProtectedRoute><HomePage /></ProtectedRoute>
                            }
                        ]
                    },
                    {
                        path: "*",
                        element: <Navigate to="/" replace />
                    }
                ]
            }
        ] satisfies AppRouteObject[]
    )
}