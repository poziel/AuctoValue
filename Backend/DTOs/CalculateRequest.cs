using System.ComponentModel.DataAnnotations;
using AuctoValue.Backend.Models;

namespace AuctoValue.Backend.DTOs;

/// <summary>
/// Request DTO for calculating auction fees.
/// </summary>
public class CalculateRequest
{
    /// <summary>
    /// The base price of the vehicle
    /// </summary>
    [Required]
    [Range(0.01, float.MaxValue, ErrorMessage = "Vehicle price must be greater than zero")]
    public float VehiclePrice { get; set; }
    
    /// <summary>
    /// The type of vehicle
    /// </summary>
    [Required]
    public VehicleType VehicleType { get; set; }
}
