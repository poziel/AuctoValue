import type { VehicleType } from './VehicleType'

/**
 * Request DTO for calculating auction fees.
 */
export interface CalculateRequest {
  /** The base price of the vehicle */
  vehiclePrice: number

  /** The type of vehicle */
  vehicleType: VehicleType
}
