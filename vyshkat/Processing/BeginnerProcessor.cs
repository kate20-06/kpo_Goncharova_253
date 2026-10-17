public class BeginnerProcessor : IEntityProcessor
{
    private readonly BeginnerService _beginnerService;
    private readonly BeginnerList _beginnerList;

    public int Priority => 20;

    public BeginnerProcessor(
        BeginnerService beginnerService,
        BeginnerList beginnerList)
    {
        _beginnerService = beginnerService;
        _beginnerList = beginnerList;
    }

    public bool Process(IEntity item)
    {
        if (item is IBeginnerFriendly beginnerItem &&
            _beginnerService.IsSuitable(beginnerItem))
        {
            _beginnerList.Add(beginnerItem);
        }

        return true;
    }
}
