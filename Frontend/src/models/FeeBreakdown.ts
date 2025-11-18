export interface FeeBreakdown {
  /** Base buyer fee */
  baseFee: number

  /** Special seller fee */
  specialFee: number

  /** Association fee based on vehicle price ranges */
  associationFee: number

  /** Fixed storage fee */
  storageFee: number

  /** Total of all fees */
  totalFees: number

  /** Grand total (vehicle price + all fees) */
  grandTotal: number
}
