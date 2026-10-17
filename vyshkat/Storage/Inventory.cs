public class Inventory
{
    private readonly List<IInvent> _items = new();

    public void Add(IInvent item)
    {
        _items.Add(item);
    }

    public IReadOnlyList<IInvent> GetAll()
    {
        return _items.AsReadOnly();
    }
}
