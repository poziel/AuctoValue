using AuctoValue.Backend.DTOs;
using AuctoValue.Backend.Models;
using AuctoValue.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuctoValue.Backend.Controllers;

/// <summary>
/// API Controller for auction fee calculations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuctionController(IAuctionCalculatorService calculatorService, ILogger<AuctionController> logger) : ControllerBase
{
    /// <summary>
    /// Calculate auction fees for a vehicle.
    /// </summary>
    /// <param name="request">The calculation request containing vehicle price and type</param>
    /// <returns>Complete fee breakdown</returns>
    /// <response code="200">Returns the calculated fee breakdown</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(FeeBreakdown), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<FeeBreakdown> Calculate([FromBody] CalculateRequest request)
    {
        try
        {
            logger.LogInformation("Calculating fees for {VehicleType} vehicle with price {VehiclePrice}", request.VehicleType, request.VehiclePrice);
            FeeBreakdown result = calculatorService.CalculateFees(request.VehiclePrice, request.VehicleType);
            logger.LogInformation("Calculation complete. Grand total: {GrandTotal}", result.GrandTotal);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid calculation request");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during calculation");
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    /// <summary>
    /// Health check endpoint.
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}