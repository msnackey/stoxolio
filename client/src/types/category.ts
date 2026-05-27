import type Stock from './stock'

export default interface Category {
  id: number
  name: string
  target: number
  stocks: Stock[]
}
