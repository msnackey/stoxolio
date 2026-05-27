import { useMatches } from "react-router-dom";
import type RouteHandle from "./router.types";

export function useRouteHandles(): RouteHandle[] {
    return useMatches()
        .map((match) => match.handle)
        .filter((h): h is RouteHandle => Boolean(h));
}