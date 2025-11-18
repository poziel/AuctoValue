using AuctoValue.Backend.Models;
using AuctoValue.Backend.Services;
using Microsoft.Extensions.Configuration;

namespace AuctoValue.Tests;

[TestClass]
public sealed class AuctionCalculatorServiceTests
{
    private IAuctionCalculatorService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _service = new AuctionCalculatorService(configuration);
    }
    
    #region Basic Tests Cases
    
    [TestMethod]
    [TestCategory("BasicTests")]
    public void Calculate_CommonVehicle_398_ReturnsCorrectFees()
    {
        // Arrange
        float vehiclePrice = 398.00f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(39.80f, result.BaseFee, "Base fee should be 39.80");
        Assert.AreEqual(7.96f, result.SpecialFee, "Special fee should be 7.96");
        Assert.AreEqual(5.00f, result.AssociationFee, "Association fee should be 5.00");
        Assert.AreEqual(100.00f, result.StorageFee, "Storage fee should be 100.00");
        Assert.AreEqual(550.76f, result.GrandTotal, "Grand total should be 550.76");
    }

    [TestMethod]
    [TestCategory("BasicTests")]
    public void Calculate_CommonVehicle_501_ReturnsCorrectFees()
    {
        // Arrange
        float vehiclePrice = 501.00f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(50.00f, result.BaseFee, "Base fee should be capped at 50.00");
        Assert.AreEqual(10.02f, result.SpecialFee, "Special fee should be 10.02");
        Assert.AreEqual(10.00f, result.AssociationFee, "Association fee should be 10.00");
        Assert.AreEqual(100.00f, result.StorageFee, "Storage fee should be 100.00");
        Assert.AreEqual(671.02f, result.GrandTotal, "Grand total should be 671.02");
    }

    [TestMethod]
    [TestCategory("BasicTests")]
    public void Calculate_CommonVehicle_57_ReturnsCorrectFees()
    {
        // Arrange
        float vehiclePrice = 57.00f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(10.00f, result.BaseFee, "Base fee should be minimum 10.00");
        Assert.AreEqual(1.14f, result.SpecialFee, "Special fee should be 1.14");
        Assert.AreEqual(5.00f, result.AssociationFee, "Association fee should be 5.00");
        Assert.AreEqual(100.00f, result.StorageFee, "Storage fee should be 100.00");
        Assert.AreEqual(173.14f, result.GrandTotal, "Grand total should be 173.14");
    }

    [TestMethod]
    [TestCategory("BasicTests")]
    public void Calculate_LuxuryVehicle_1800_ReturnsCorrectFees()
    {
        // Arrange
        float vehiclePrice = 1800.00f;
        VehicleType vehicleType = VehicleType.Luxury;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(180.00f, result.BaseFee, "Base fee should be 180.00");
        Assert.AreEqual(72.00f, result.SpecialFee, "Special fee should be 72.00");
        Assert.AreEqual(15.00f, result.AssociationFee, "Association fee should be 15.00");
        Assert.AreEqual(100.00f, result.StorageFee, "Storage fee should be 100.00");
        Assert.AreEqual(2167.00f, result.GrandTotal, "Grand total should be 2167.00");
    }

    [TestMethod]
    [TestCategory("BasicTests")]
    public void Calculate_CommonVehicle_1100_ReturnsCorrectFees()
    {
        // Arrange
        float vehiclePrice = 1100.00f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(50.00f, result.BaseFee, "Base fee should be capped at 50.00");
        Assert.AreEqual(22.00f, result.SpecialFee, "Special fee should be 22.00");
        Assert.AreEqual(15.00f, result.AssociationFee, "Association fee should be 15.00");
        Assert.AreEqual(100.00f, result.StorageFee, "Storage fee should be 100.00");
        Assert.AreEqual(1287.00f, result.GrandTotal, "Grand total should be 1287.00");
    }

    [TestMethod]
    [TestCategory("BasicTests")]
    public void Calculate_LuxuryVehicle_1000000_ReturnsCorrectFees()
    {
        // Arrange
        float vehiclePrice = 1000000.00f;
        VehicleType vehicleType = VehicleType.Luxury;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(200.00f, result.BaseFee, "Base fee should be capped at 200.00");
        Assert.AreEqual(40000.00f, result.SpecialFee, "Special fee should be 40000.00");
        Assert.AreEqual(20.00f, result.AssociationFee, "Association fee should be 20.00");
        Assert.AreEqual(100.00f, result.StorageFee, "Storage fee should be 100.00");
        Assert.AreEqual(1040320.00f, result.GrandTotal, "Grand total should be 1040320.00");
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [TestMethod]
    [TestCategory("EdgeCases")]
    public void Calculate_VerySmallPrice_AppliesMinimumBaseFee()
    {
        // Arrange
        float vehiclePrice = 1.00f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(10.00f, result.BaseFee, "Base fee should be minimum 10 for common");
        Assert.AreEqual(0.02f, result.SpecialFee);
        Assert.AreEqual(5.00f, result.AssociationFee);
    }

    [TestMethod]
    [TestCategory("EdgeCases")]
    public void Calculate_LuxuryMinimumBaseFee_AppliesCorrectly()
    {
        // Arrange
        float vehiclePrice = 100.00f;
        VehicleType vehicleType = VehicleType.Luxury;

        // Act
        var result = _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert
        Assert.AreEqual(25.00f, result.BaseFee, "Base fee should be minimum 25 for luxury");
    }

    [TestMethod]
    [TestCategory("EdgeCases")]
    [ExpectedException(typeof(ArgumentException))]
    public void Calculate_ZeroPrice_ThrowsException()
    {
        // Arrange
        float vehiclePrice = 0f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert - Exception expected
    }

    [TestMethod]
    [TestCategory("EdgeCases")]
    [ExpectedException(typeof(ArgumentException))]
    public void Calculate_NegativePrice_ThrowsException()
    {
        // Arrange
        float vehiclePrice = -100f;
        VehicleType vehicleType = VehicleType.Common;

        // Act
        _service.CalculateFees(vehiclePrice, vehicleType);

        // Assert - Exception expected
    }

    #endregion

    #region Association Fee Boundary Tests

    [TestMethod]
    [TestCategory("AssociationFees")]
    public void Calculate_AssociationFee_At500_Returns5()
    {
        // Arrange
        var result = _service.CalculateFees(500f, VehicleType.Common);
        
        // Assert
        Assert.AreEqual(5.00f, result.AssociationFee);
    }

    [TestMethod]
    [TestCategory("AssociationFees")]
    public void Calculate_AssociationFee_At501_Returns10()
    {
        // Arrange
        var result = _service.CalculateFees(501f, VehicleType.Common);
        
        // Assert
        Assert.AreEqual(10.00f, result.AssociationFee);
    }

    [TestMethod]
    [TestCategory("AssociationFees")]
    public void Calculate_AssociationFee_At1000_Returns10()
    {
        // Arrange
        var result = _service.CalculateFees(1000f, VehicleType.Common);
        
        // Assert
        Assert.AreEqual(10.00f, result.AssociationFee);
    }

    [TestMethod]
    [TestCategory("AssociationFees")]
    public void Calculate_AssociationFee_At1001_Returns15()
    {
        // Arrange
        var result = _service.CalculateFees(1001f, VehicleType.Common);
        
        // Assert
        Assert.AreEqual(15.00f, result.AssociationFee);
    }

    [TestMethod]
    [TestCategory("AssociationFees")]
    public void Calculate_AssociationFee_At3000_Returns15()
    {
        // Arrange
        var result = _service.CalculateFees(3000f, VehicleType.Common);
        
        // Assert
        Assert.AreEqual(15.00f, result.AssociationFee);
    }

    [TestMethod]
    [TestCategory("AssociationFees")]
    public void Calculate_AssociationFee_At3001_Returns20()
    {
        // Arrange
        var result = _service.CalculateFees(3001f, VehicleType.Common);
        
        // Assert
        Assert.AreEqual(20.00f, result.AssociationFee);
    }

    #endregion
}
