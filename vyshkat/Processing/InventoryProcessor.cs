public class InventoryProcessor : IEntityProcessor
{
    private readonly Inventory _inventory;

    public int Priority => 10;

    public InventoryProcessor(Inventory inventory)
    {
        _inventory = inventory;
    }

    public bool Process(IEntity item)
    {
        if (item is IInvent inventoryItem)
        {
            _inventory.Add(inventoryItem);
        }

        return true;
    }
}
