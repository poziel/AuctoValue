<template>
  <main
    class="min-h-screen bg-gradient-to-b from-[#FFFDF8] to-[#F7F3E8] dark:from-[#131313] dark:to-[#1A1A1A] flex flex-col"
  >
    <div class="flex-1 flex flex-col items-center pt-30 pb-16">
      <!-- Logo -->
      <div class="flex flex-col items-center text-center space-y-2">
        <img
          src="./assets/logo.svg"
          alt="AuctoValue Logo"
          class="h-20 w-auto drop-shadow-[0_6px_12px_rgba(0,0,0,0.25)]"
        />
        <p class="text-base text-slate-500 dark:text-slate-300">
          A simple auction fee calculator for vehicles.
        </p>
      </div>

      <!-- Cards -->
      <div class="mt-8 w-full max-w-4xl px-4 grid gap-6 md:grid-cols-2">
        <section
          class="rounded-2xl bg-white dark:bg-[#151515] border border-[#E6DFCF] dark:border-[#2A2A2A] shadow-sm shadow-black/5 dark:shadow-black/40 p-6"
        >
          <h2 class="mb-4 text-lg font-semibold text-slate-900 dark:text-slate-50">
            Vehicle information
          </h2>

          <div class="space-y-4">
            <!-- Vehicle price -->
            <div class="flex flex-col gap-1">
              <label
                for="vehiclePrice"
                class="text-sm font-medium text-slate-700 dark:text-slate-200"
              >
                Vehicle price ($)
              </label>
              <input
                id="vehiclePrice"
                v-model.number="form.vehiclePrice"
                type="number"
                min="0"
                step="0.01"
                class="rounded-md border border-slate-300 dark:border-slate-600 bg-white dark:bg-[#101010] px-3 py-2 text-sm text-slate-900 dark:text-slate-50 shadow-sm outline-none ring-0 transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500 dark:focus:border-indigo-400 dark:focus:ring-indigo-400"
              />
            </div>

            <!-- Vehicle type -->
            <div class="flex flex-col gap-1">
              <label
                for="vehicleType"
                class="text-sm font-medium text-slate-700 dark:text-slate-200"
              >
                Vehicle type
              </label>
              <select
                id="vehicleType"
                v-model.number="form.vehicleType"
                class="rounded-md border border-slate-300 dark:border-slate-600 bg-white dark:bg-[#101010] px-3 py-2 text-sm text-slate-900 dark:text-slate-50 shadow-sm outline-none ring-0 transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500 dark:focus:border-indigo-400 dark:focus:ring-indigo-400"
              >
                <option :value="VehicleType.Common">Common</option>
                <option :value="VehicleType.Luxury">Luxury</option>
              </select>
            </div>

            <!-- Error message -->
            <p
              v-if="apiError"
              class="rounded-md bg-red-50 dark:bg-red-900/40 px-3 py-2 text-sm text-red-700 dark:text-red-200"
            >
              {{ apiError }}
            </p>

            <p v-else-if="loading" class="text-sm text-slate-500 dark:text-slate-400">
              Calculating fees…
            </p>
          </div>
        </section>

        <section
          class="rounded-2xl bg-white dark:bg-[#151515] border border-[#E6DFCF] dark:border-[#2A2A2A] shadow-sm shadow-black/5 dark:shadow-black/40 p-6"
        >
          <h2 class="mb-4 text-lg font-semibold text-slate-900 dark:text-slate-50">
            Fee breakdown
          </h2>

          <p v-if="!feeBreakdown" class="text-sm text-slate-500 dark:text-slate-400">
            Enter a price and vehicle type to see the detailed fees.
          </p>

          <div v-else class="space-y-4">
            <!-- Small fee chips -->
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div
                class="inline-flex items-center justify-between gap-2 rounded-full bg-white dark:bg-[#1E1E1E] px-3 py-1.5 text-xs font-medium text-slate-700 dark:text-slate-100 shadow-sm border border-slate-100 dark:border-slate-700"
              >
                <span>Base fee</span>
                <span class="font-mono font-semibold">
                  {{ formatCurrency(feeBreakdown.baseFee) }}
                </span>
              </div>

              <div
                class="inline-flex items-center justify-between gap-2 rounded-full bg-white dark:bg-[#1E1E1E] px-3 py-1.5 text-xs font-medium text-slate-700 dark:text-slate-100 shadow-sm border border-slate-100 dark:border-slate-700"
              >
                <span>Special fee</span>
                <span class="font-mono font-semibold truncate">
                  {{ formatCurrency(feeBreakdown.specialFee) }}
                </span>
              </div>

              <div
                class="inline-flex items-center justify-between gap-2 rounded-full bg-white dark:bg-[#1E1E1E] px-3 py-1.5 text-xs font-medium text-slate-700 dark:text-slate-100 shadow-sm border border-slate-100 dark:border-slate-700"
              >
                <span>Association fee</span>
                <span class="font-mono font-semibold">
                  {{ formatCurrency(feeBreakdown.associationFee) }}
                </span>
              </div>

              <div
                class="inline-flex items-center justify-between gap-2 rounded-full bg-white dark:bg-[#1E1E1E] px-3 py-1.5 text-xs font-medium text-slate-700 dark:text-slate-100 shadow-sm border border-slate-100 dark:border-slate-700"
              >
                <span>Storage fee</span>
                <span class="font-mono font-semibold">
                  {{ formatCurrency(feeBreakdown.storageFee) }}
                </span>
              </div>
            </div>

            <!-- Total fees -->
            <div
              class="rounded-md bg-gradient-to-r from-[#E9D68F] to-[#D9C36B] text-[#3E3200] dark:from-[#8A6D1A] dark:to-[#A78622] dark:text-[#FFF4D2] px-4 py-3 font-semibold flex justify-between"
            >
              <span>Total fees</span>
              <span class="font-mono truncate">
                {{ formatCurrency(feeBreakdown.totalFees) }}
              </span>
            </div>

            <!-- Grand total -->
            <div
              class="rounded-md bg-gradient-to-r from-[#D9C36B] to-[#C5A850] text-[#2F2600] dark:from-[#A78622] dark:to-[#C39A32] dark:text-[#FFF9E6] px-4 py-3 font-semibold flex justify-between"
            >
              <span>Grand total</span>
              <span class="font-mono truncate">
                {{ formatCurrency(feeBreakdown.grandTotal) }}
              </span>
            </div>
          </div>
        </section>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { reactive, ref, watch } from 'vue'
import type { CalculateRequest } from '@/models/CalculateRequest'
import type { FeeBreakdown } from '@/models/FeeBreakdown'
import { VehicleType } from '@/models/VehicleType'
import { calculateFees } from '@/services/feeApi'

const form = reactive<CalculateRequest>({
  vehiclePrice: 0,
  vehicleType: VehicleType.Common,
})

const feeBreakdown = ref<FeeBreakdown | null>(null)
const loading = ref(false)
const apiError = ref<string | null>(null)

let lastRequestId = 0

/**
 * Refresh the fees based on the form input.
 *
 * @returns A promise that resolves when the fees are refreshed.
 */
async function refreshFees() {
  apiError.value = null

  if (form.vehiclePrice <= 0) {
    feeBreakdown.value = null
    loading.value = false
    return
  }

  const requestId = ++lastRequestId
  loading.value = true

  try {
    const payload: CalculateRequest = {
      vehiclePrice: form.vehiclePrice,
      vehicleType: form.vehicleType,
    }

    const result = await calculateFees(payload)

    if (requestId === lastRequestId) {
      feeBreakdown.value = result
    }
  } catch (err: unknown) {
    if (requestId === lastRequestId) {
      feeBreakdown.value = null
      apiError.value = err instanceof Error ? err.message : 'An unknown error occurred.'
    }
  } finally {
    if (requestId === lastRequestId) {
      loading.value = false
    }
  }
}

watch(
  () => [form.vehiclePrice, form.vehicleType],
  () => {
    void refreshFees()
  }
)

/**
 * Format a number as a currency string.
 *
 * @param value - The number to format.
 * @returns The formatted currency string.
 */
function formatCurrency(value: number): string {
  if (Number.isNaN(value)) return '-'
  return value.toLocaleString('en-CA', {
    style: 'currency',
    currency: 'CAD',
    maximumFractionDigits: 2,
  })
}
</script>
