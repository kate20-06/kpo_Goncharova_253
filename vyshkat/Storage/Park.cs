public class Park
{
    private readonly List<ITransport> _items = new();

    public void Add(ITransport item)
    {
        _items.Add(item);
    }

    public IReadOnlyList<ITransport> GetAll()
    {
        return _items.AsReadOnly();
    }
}
