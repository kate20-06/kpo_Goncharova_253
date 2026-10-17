public class EnergyConsumptionServiceTests
{
    [Fact]
    public void CalculateTotal_ReturnsCorrectSum()
    {
        var service = new EnergyConsumptionService();
        var consumers = new List<IEnergyConsumer>
        {
            new Scooter { DailyEnergyConsumption = 2.5 },
            new ChargingStation { DailyEnergyConsumption = 5.5 }
        };

        var result = service.CalculateTotal(consumers);

        Assert.Equal(8.0, result);
    }
}
