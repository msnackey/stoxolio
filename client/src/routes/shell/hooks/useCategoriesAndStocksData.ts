import { useRouteLoaderData } from 'react-router-dom'
import type categoriesAndStocksLoader from '../loaders/categoriesAndStocksLoader'

type CategoriesAndStocksData = Awaited<ReturnType<typeof categoriesAndStocksLoader>>

export default function useCategoriesAndStocksData() {
  return useRouteLoaderData('shell') as CategoriesAndStocksData
}
