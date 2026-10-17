using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public class ConsoleApp
{
    private readonly EntityFactory _factory;
    private readonly Park _park;
    private readonly Inventory _inventory;
    private readonly BeginnerList _beginnerList;
    private readonly EnergyConsumptionService _energyService;
    private readonly ConsoleInput _input;

    public ConsoleApp(
        EntityFactory factory,
        Park park,
        Inventory inventory,
        BeginnerList beginnerList,
        EnergyConsumptionService energyService,
        ConsoleInput input)
    {
        _factory = factory;
        _park = park;
        _inventory = inventory;
        _beginnerList = beginnerList;
        _energyService = energyService;
        _input = input;
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();

            var choice = Console.ReadLine();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddScooter();
                    break;

                case "2":
                    AddHelmet();
                    break;

                case "3":
                    AddChargingStation();
                    break;

                case "4":
                    ShowPark();
                    break;

                case "5":
                    ShowInventory();
                    break;

                case "6":
                    ShowBeginnerTransport();
                    break;

                case "7":
                    ShowEnergyConsumption();
                    break;

                case "0":
                    Console.WriteLine("Завершение работы.");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== ВышКат ===");
        Console.WriteLine("1. Добавить самокат");
        Console.WriteLine("2. Добавить шлем");
        Console.WriteLine("3. Добавить зарядную станцию");
        Console.WriteLine("4. Показать парк");
        Console.WriteLine("5. Показать инвентарь");
        Console.WriteLine("6. Показать транспорт для новичков");
        Console.WriteLine("7. Показать энергопотребление");
        Console.WriteLine("0. Выход");

        Console.Write("Выберите действие: ");
    }

    private void AddScooter()
    {
        Console.WriteLine("=== Новый самокат ===");

        var name = _input.ReadString("Название: ");
        var inventNumber =
            _input.ReadInt("Инвентарный номер: ");

        var beginnerEase =
            _input.ReadIntInRange(
                "Удобство для новичка (1-10): ",
                1,
                10);

        var dailyEnergyConsumption =
            _input.ReadDouble(
                "Энергопотребление в день: ");

        var quality =
            _input.ReadIntInRange(
                "Качество (1-10): ",
                1,
                10);

        var scooter = _factory.Create(() => new Scooter
        {
            Name = name,
            InventNumber = inventNumber,
            BeginnerEase = beginnerEase,
            DailyEnergyConsumption = dailyEnergyConsumption,
            Quality = quality
        });

        if (_park.GetAll().Contains(scooter))
        {
            Console.WriteLine(
                "Самокат прошёл техосмотр и добавлен в парк.");
        }
        else
        {
            Console.WriteLine(
                "Самокат не прошёл техосмотр.");
        }
    }

    private void AddHelmet()
    {
        Console.WriteLine("=== Новый шлем ===");

        var name = _input.ReadString("Название: ");
        var inventNumber =
            _input.ReadInt("Инвентарный номер: ");

        _factory.Create(() => new Helmet
        {
            Name = name,
            InventNumber = inventNumber
        });

        Console.WriteLine(
            "Шлем добавлен в инвентарь.");
    }

    private void AddChargingStation()
    {
        Console.WriteLine(
            "=== Новая зарядная станция ===");

        var name = _input.ReadString("Название: ");
        var inventNumber =
            _input.ReadInt("Инвентарный номер: ");

        var dailyEnergyConsumption =
            _input.ReadDouble(
                "Энергопотребление в день: ");

        _factory.Create(() => new ChargingStation
        {
            Name = name,
            InventNumber = inventNumber,
            DailyEnergyConsumption = dailyEnergyConsumption
        });

        Console.WriteLine(
            "Зарядная станция добавлена в инвентарь.");
    }

    private void ShowPark()
    {
        Console.WriteLine("=== Парк ===");

        var transport = _park.GetAll();

        if (transport.Count == 0)
        {
            Console.WriteLine("Парк пуст.");
            return;
        }

        foreach (var item in transport)
        {
            if (item is IInvent inventItem)
            {
                Console.WriteLine(
                    $"{inventItem.Name}, " +
                    $"инвентарный номер: " +
                    $"{inventItem.InventNumber}");
            }
        }
    }

    private void ShowInventory()
    {
        Console.WriteLine("=== Инвентарь ===");

        var items = _inventory.GetAll();

        if (items.Count == 0)
        {
            Console.WriteLine("Инвентарь пуст.");
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine(
                $"{item.Name}, " +
                $"инвентарный номер: " +
                $"{item.InventNumber}");
        }
    }

    private void ShowBeginnerTransport()
    {
        Console.WriteLine(
            "=== Транспорт для новичков ===");

        var items = _beginnerList.GetAll();

        if (items.Count == 0)
        {
            Console.WriteLine(
                "Подходящего транспорта пока нет.");
            return;
        }

        foreach (var item in items)
        {
            if (item is IInvent inventItem)
            {
                Console.WriteLine(
                    $"{inventItem.Name}, " +
                    $"уровень удобства: " +
                    $"{item.BeginnerEase}");
            }
        }
    }

    private void ShowEnergyConsumption()
    {
        Console.WriteLine(
            "=== Энергопотребление ===");

        var consumers = _inventory
            .GetAll()
            .OfType<IEnergyConsumer>();

        var total =
            _energyService.CalculateTotal(consumers);

        Console.WriteLine(
            $"Всего: {total} кВт·ч в день");
    }
}