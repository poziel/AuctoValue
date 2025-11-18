import type { CalculateRequest } from '@/models/CalculateRequest'
import type { FeeBreakdown } from '@/models/FeeBreakdown'

const API_BASE_URL = import.meta.env.CSHARP_API_BASE_URL as string | undefined

/**
 * Get the base URL for the API from environment variables.
 *
 * @returns The base URL for the API.
 * @throws {Error} If the API base URL is not defined in environment variables.
 */
function getApiBaseUrl(): string {
  if (!API_BASE_URL) {
    throw new Error('CSHARP_API_BASE_URL is not defined in your environment.')
  }
  return API_BASE_URL.replace(/\/+$/, '') // trim trailing slash
}

/**
 * Calculate the fees for a vehicle.
 *
 * @param payload - The vehicle fee request payload.
 * @returns A promise that resolves to the calculated fees.
 * @throws {Error} If the API call fails.
 */
export async function calculateFees(payload: CalculateRequest): Promise<FeeBreakdown> {
  const response = await fetch(`${getApiBaseUrl()}/api/auction/calculate`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  })

  if (!response.ok) {
    // You can improve this with better error handling
    throw new Error(`Failed to calculate fees (status ${response.status})`)
  }

  return (await response.json()) as FeeBreakdown
}
