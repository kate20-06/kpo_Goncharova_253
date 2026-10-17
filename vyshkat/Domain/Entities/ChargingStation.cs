public class ChargingStation : IEnergyConsumer, IInvent
{
    public string Name { get; set; } = "ChargingStation";
    public int InventNumber { get; set; }
    public double DailyEnergyConsumption { get; set; }
}
