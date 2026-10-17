public class Scooter : ITransport, IEnergyConsumer, IInvent, IBeginnerFriendly
{
    public string Name { get; set; } = "Scooter";
    public double DailyEnergyConsumption { get; set; }
    public int InventNumber { get; set; }
    public double Quality { get; set; }
    public int BeginnerEase { get; set; }
}
