import { LoadingOverlay } from "@mantine/core";
import { Outlet, useFetchers, useNavigation } from "react-router-dom";

export default function RootLayout() {
    const navigation = useNavigation();
    const fetchers = useFetchers();

    const isBusy = navigation.state !== "idle" || fetchers.some(f => f.state !== "idle");

    return (
        <>
            <LoadingOverlay
                visible={isBusy}
                zIndex={1000}
                overlayProps={{ blur: 2 }}
                loaderProps={{ size: "lg" }}
            />

            <Outlet />
        </>
    )
}