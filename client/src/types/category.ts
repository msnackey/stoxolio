import type Stock from './stock'

export default interface Category {
  id: number
  name: string
  value: number
  target: number
  actual: number
  stocks: Stock[]
}
