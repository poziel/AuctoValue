using AuctoValue.Backend.Models;

namespace AuctoValue.Backend.Services;

public interface IAuctionCalculatorService
{
    /// <summary>
    /// Calculates the complete fee breakdown for a vehicle auction.
    /// </summary>
    /// <param name="vehiclePrice">The base price of the vehicle</param>
    /// <param name="vehicleType">The type of vehicle</param>
    /// <returns>Complete fee breakdown of the vehicle auction</returns>
    FeeBreakdown CalculateFees(float vehiclePrice, VehicleType vehicleType);
}