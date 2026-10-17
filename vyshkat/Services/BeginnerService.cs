public class BeginnerService
{
    public bool IsSuitable(IBeginnerFriendly item)
    {
        return item.BeginnerEase >= 6;
    }
}
