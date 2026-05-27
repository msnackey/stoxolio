export default interface Stock {
  id: number
  name: string
  ticker: string
  exchange: string
  sri: boolean
  shares: number
  price: number
  invest: boolean
  categoryId: number
  prevPrice: number
  value: number
  priceChange: number
  valueChange: number
}
