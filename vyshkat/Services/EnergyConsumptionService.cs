public class EnergyConsumptionService
{
    public double CalculateTotal(IEnumerable<IEnergyConsumer> consumers)
    {
        return consumers.Sum(consumer => consumer.DailyEnergyConsumption);
    }
}
