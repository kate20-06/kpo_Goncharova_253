public class TransportProcessor : IEntityProcessor
{
    private readonly IServiceCenter _serviceCenter;
    private readonly Park _park;

    public int Priority => 0;

    public TransportProcessor(
        IServiceCenter serviceCenter,
        Park park)
    {
        _serviceCenter = serviceCenter;
        _park = park;
    }

    public bool Process(IEntity item)
    {
        if (item is not ITransport transport)
        {
            return true;
        }

        if (!_serviceCenter.Inspect(transport))
        {
            return false;
        }

        _park.Add(transport);
        return true;
    }
}
