public class EntityFactory
{
    private readonly IReadOnlyList<IEntityProcessor> _processors;

    public EntityFactory(IEnumerable<IEntityProcessor> processors)
    {
        _processors = processors
            .OrderBy(processor => processor.Priority)
            .ToList();
    }

    public T Create<T>(Func<T> creator)
        where T : IEntity
    {
        var item = creator();

        foreach (var processor in _processors)
        {
            if (!processor.Process(item))
            {
                break;
            }
        }

        return item;
    }
}
