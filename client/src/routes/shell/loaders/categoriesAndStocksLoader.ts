import fetchCategories from "../api/fetchCategories";
import fetchStocks from "../api/fetchStocks";
import type { AxiosError } from "axios";

export default async function categoriesAndStocksLoader() {
    try {
        const [categories, stocks] = await Promise.all([fetchCategories(), fetchStocks()]);
        return {
            categories: categories.categories,
            stocks: stocks.stocks
        };
    }
    catch (_error) {
        const error = _error as AxiosError;
        throw new Response("Failed to load categories and stocks", { status: error.status ?? 500, statusText: error.message });
    }
}