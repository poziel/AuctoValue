using AuctoValue.Backend.Models;

namespace AuctoValue.Backend.Services;

/// <summary>
/// Service for calculating the complete fee breakdown for a vehicle auction.
/// </summary>
/// <param name="configuration">Inject the configuration to get fees from appsettings.</param>
public class AuctionCalculatorService(IConfiguration configuration) : IAuctionCalculatorService
{
    #region Fields

    // Configurable fee values (loaded from appsettings.json -> Fees section with sane defaults)
    private readonly int _storageFee = configuration.GetValue("Fees:StorageFee", 100);
    private readonly float _baseFeePercentage = configuration.GetValue("Fees:BaseFeePercentage", 0.10f);

    // Base fee limits for common vehicles
    private readonly int _commonBaseFeeMin = configuration.GetValue("Fees:CommonBaseFeeMin", 10);
    private readonly int _commonBaseFeeMax = configuration.GetValue("Fees:CommonBaseFeeMax", 50);

    // Base fee limits for luxury vehicles
    private readonly int _luxuryBaseFeeMin = configuration.GetValue("Fees:LuxuryBaseFeeMin", 25);
    private readonly int _luxuryBaseFeeMax = configuration.GetValue("Fees:LuxuryBaseFeeMax", 200);

    // Special fee percentages
    private readonly float _commonSpecialFeePercentage = configuration.GetValue("Fees:CommonSpecialFeePercentage", 0.02f);
    private readonly float _luxurySpecialFeePercentage = configuration.GetValue("Fees:LuxurySpecialFeePercentage", 0.04f);

    #endregion

    #region Methods
    
    /// <summary>
    /// Calculates the complete fee breakdown for a vehicle auction
    /// </summary>
    /// <param name="vehiclePrice">The base price of the vehicle</param>
    /// <param name="vehicleType">The type of vehicle (Common or Luxury)</param>
    /// <returns>Complete fee breakdown including all calculated fees</returns>
    /// <exception cref="ArgumentException">Thrown when the vehicle price is negative or zero</exception>
    /// <exception cref="ArgumentException">Thrown when the vehicle type is not recognized</exception>
    public FeeBreakdown CalculateFees(float vehiclePrice, VehicleType vehicleType)
    {
        if (vehiclePrice <= 0)
        {
            throw new ArgumentException("Vehicle price must be greater than zero", nameof(vehiclePrice));
        }

        float baseFee = CalculateBaseFee(vehiclePrice, vehicleType);
        float specialFee = CalculateSpecialFee(vehiclePrice, vehicleType);
        byte associationFee = CalculateAssociationFee(vehiclePrice);

        float totalFees = baseFee + specialFee + associationFee + _storageFee;
        float grandTotal = vehiclePrice + totalFees;

        return new FeeBreakdown
        {
            BaseFee = (float)Math.Round(baseFee, 2),
            SpecialFee = (float)Math.Round(specialFee, 2),
            AssociationFee = associationFee,
            StorageFee = _storageFee,
            TotalFees = (float)Math.Round(totalFees, 2),
            GrandTotal = (float)Math.Round(grandTotal, 2),
        };
    }
    
    /// <summary>
    /// Calculates the base buyer fee based on the type of vehicle.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the vehicle type is not recognized</exception>
    private float CalculateBaseFee(float vehiclePrice, VehicleType vehicleType)
    {
        float baseFee = vehiclePrice * _baseFeePercentage;
        
        return vehicleType switch
        {
            VehicleType.Common => Math.Clamp(baseFee, _commonBaseFeeMin, _commonBaseFeeMax),
            VehicleType.Luxury => Math.Clamp(baseFee, _luxuryBaseFeeMin, _luxuryBaseFeeMax),
            _ => throw new ArgumentException($"Unknown vehicle type: {vehicleType}", nameof(vehicleType))
        };
    }

    /// <summary>
    /// Calculates the special seller fee based on the type of vehicle.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the vehicle type is not recognized</exception>
    private float CalculateSpecialFee(float vehiclePrice, VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Common => vehiclePrice * _commonSpecialFeePercentage,
            VehicleType.Luxury => vehiclePrice * _luxurySpecialFeePercentage,
            _ => throw new ArgumentException($"Unknown vehicle type: {vehicleType}", nameof(vehicleType))
        };
    }

    /// <summary>
    /// Calculates the association fee based on vehicle price ranges.
    /// </summary>
    private static byte CalculateAssociationFee(float vehiclePrice)
    {
        return vehiclePrice switch
        {
            <= 500 => 5,
            <= 1000 => 10,
            <= 3000 => 15,
            _ => 20
        };
    }

    #endregion
}