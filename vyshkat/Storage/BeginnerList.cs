public class BeginnerList
{
    private readonly List<IBeginnerFriendly> _items = new();

    public void Add(IBeginnerFriendly item)
    {
        _items.Add(item);
    }

    public IReadOnlyList<IBeginnerFriendly> GetAll()
    {
        return _items.AsReadOnly();
    }
}
