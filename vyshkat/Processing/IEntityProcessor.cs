public interface IEntityProcessor
{
    int Priority { get; }

    bool Process(IEntity item);
}