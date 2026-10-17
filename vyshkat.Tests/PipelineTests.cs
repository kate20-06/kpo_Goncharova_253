public class PipelineTests
{
    [Fact]
    public void AcceptedScooter_GoesToAllRelevantCollections()
    {
        var park = new Park();
        var inventory = new Inventory();
        var beginnerList = new BeginnerList();

        var processors = new List<IEntityProcessor>
        {
            new TransportProcessor(new FakeServiceCenter(true), park),
            new InventoryProcessor(inventory),
            new BeginnerProcessor(new BeginnerService(), beginnerList)
        };

        var factory = new EntityFactory(processors);

        var scooter = factory.Create(() => new Scooter
        {
            Name = "Campus scooter",
            InventNumber = 501,
            BeginnerEase = 7,
            DailyEnergyConsumption = 2.4
        });

        Assert.Same(scooter, Assert.Single(park.GetAll()));
        Assert.Same(scooter, Assert.Single(inventory.GetAll()));
        Assert.Same(scooter, Assert.Single(beginnerList.GetAll()));
    }

    [Fact]
    public void RejectedScooter_StopsBeforeInventoryAndBeginnerList()
    {
        var park = new Park();
        var inventory = new Inventory();
        var beginnerList = new BeginnerList();

        var processors = new List<IEntityProcessor>
        {
            new BeginnerProcessor(new BeginnerService(), beginnerList),
            new InventoryProcessor(inventory),
            new TransportProcessor(new FakeServiceCenter(false), park)
        };

        var factory = new EntityFactory(processors);

        factory.Create(() => new Scooter
        {
            Name = "Suspicious scooter",
            InventNumber = 777,
            BeginnerEase = 10
        });

        Assert.Empty(park.GetAll());
        Assert.Empty(inventory.GetAll());
        Assert.Empty(beginnerList.GetAll());
    }

    [Fact]
    public void Helmet_SkipsTransportCheck_ButIsInventoried()
    {
        var park = new Park();
        var inventory = new Inventory();

        var processors = new List<IEntityProcessor>
        {
            new TransportProcessor(new FakeServiceCenter(false), park),
            new InventoryProcessor(inventory)
        };

        var factory = new EntityFactory(processors);

        var helmet = factory.Create(() => new Helmet
        {
            Name = "Red helmet",
            InventNumber = 42
        });

        Assert.Empty(park.GetAll());
        Assert.Same(helmet, Assert.Single(inventory.GetAll()));
    }

    [Fact]
    public void BeginnerProcessor_DoesNotAddTransportBelowThreshold()
    {
        var beginnerList = new BeginnerList();
        var processor = new BeginnerProcessor(new BeginnerService(), beginnerList);

        var scooter = new Scooter { BeginnerEase = 5 };

        bool result = processor.Process(scooter);

        Assert.True(result);
        Assert.Empty(beginnerList.GetAll());
    }
}
