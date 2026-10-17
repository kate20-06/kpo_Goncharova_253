public class BeginnerServiceTests
{
    [Theory]
    [InlineData(8, true)]
    [InlineData(6, true)]
    [InlineData(4, false)]
    public void IsSuitable_ReturnsExpectedResult(int ease, bool expected)
    {
        var service = new BeginnerService();
        var scooter = new Scooter { BeginnerEase = ease };

        var result = service.IsSuitable(scooter);

        Assert.Equal(expected, result);
    }
}