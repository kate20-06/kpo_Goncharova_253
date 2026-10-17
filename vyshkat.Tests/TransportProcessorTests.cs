public class TransportProcessorTests
{
    [Fact]
    public void Process_WhenInspectionPasses_AddsTransportToPark()
    {
        var park = new Park();
        var serviceCenter = new FakeServiceCenter(true);
        var processor = new TransportProcessor(serviceCenter, park);
        var scooter = new Scooter();

        var result = processor.Process(scooter);

        Assert.True(result);
        Assert.Single(park.GetAll());
    }

    [Fact]
    public void Process_WhenInspectionFails_DoesNotAddTransportToPark()
    {
        var park = new Park();
        var serviceCenter = new FakeServiceCenter(false);
        var processor = new TransportProcessor(serviceCenter, park);
        var scooter = new Scooter();

        var result = processor.Process(scooter);

        Assert.False(result);
        Assert.Empty(park.GetAll());
    }
}
