import type Stock from "./Stock";

export default interface Category {
    id: number;
    name: string;
    target: number;
    stocks: Stock[];
}
