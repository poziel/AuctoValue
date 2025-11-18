namespace AuctoValue.Backend.Models;

/// <summary>
/// Represents the fee breakdown calculated for a vehicule in an auction
/// </summary>
public class FeeBreakdown
{
    /// <summary>
    /// Base buyer fee
    /// </summary>
    public float BaseFee { get; init; }
    
    /// <summary>
    /// Special seller fee
    /// </summary>
    public float SpecialFee { get; init; }
    
    /// <summary>
    /// Association fee based on vehicle price ranges
    /// </summary>
    public float AssociationFee { get; init; }
    
    /// <summary>
    /// Fixed storage fee
    /// </summary>
    public float StorageFee { get; init; }
    
    /// <summary>
    /// Total of all fees
    /// </summary>
    public float TotalFees { get; init; }
    
    /// <summary>
    /// Grand total (vehicle price and all fees)
    /// </summary>
    public float GrandTotal { get; init; }
}