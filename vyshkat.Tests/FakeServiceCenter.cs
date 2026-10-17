public class FakeServiceCenter : IServiceCenter
{
    private readonly bool _result;

    public FakeServiceCenter(bool result)
    {
        _result = result;
    }

    public bool Inspect(ITransport transport)
    {
        return _result;
    }
}
