# VyshKat Fleet Manager

Консольное приложение для учёта парка кикшеринга «ВышКат».

В приложении можно добавлять транспорт и инвентарь, проводить техосмотр перед добавлением транспорта в парк, считать энергопотребление и получать список транспорта, подходящего для новичков.

## Запуск

Восстановить зависимости:

```bash
dotnet restore vyshkat.sln
```

Запустить приложение:

```bash
dotnet run --project vyshkat/vyshkat.csproj
```

Запустить тесты:

```bash
dotnet test vyshkat.sln
```

Запустить тесты с покрытием:

```bash
dotnet test vyshkat.Tests/vyshkat.Tests.csproj --collect:"XPlat Code Coverage"
```
## Структура
```
vyshkat/
├── Factories/
│   └── EntityFactory.cs
├── Inspection/
│   ├── IInspectionRule.cs
│   └── QualityInspectionRule.cs
├── Processing/
│   ├── IEntityProcessor.cs
│   ├── TransportProcessor.cs
│   ├── InventoryProcessor.cs
│   └── BeginnerProcessor.cs
├── Services/
│   ├── BeginnerService.cs
│   ├── EnergyConsumptionService.cs
│   ├── IServiceCenter.cs
│   └── ServiceCenter.cs
├── Storage/
│   ├── Park.cs
│   ├── Inventory.cs
│   └── BeginnerList.cs
├── ConsoleApp.cs
├── ConsoleInput.cs
├── DependencyInjection.cs
└── Program.cs
```
## Как устроен проект

В проекте есть конкретные сущности — самокат, шлем и зарядная станция. Общие абстракции - абстракции транспорта, инвентаря и энергопотребляющих объектов.

Разные свойства объектов вынесены в отдельные интерфейсы:

- `ITransport` — транспорт;
- `IInvent` — объект с инвентарным номером;
- `IEnergyConsumer` — объект, который потребляет энергию;
- `IBeginnerFriendly` — объект, для которого задана оценка удобства для новичка.

Это позволяет не заставлять все сущности реализовывать одинаковый набор свойств.

Создание объектов проходит через `EntityFactory`. После создания объект последовательно обрабатывается процессорами:

```text
TransportProcessor
→ InventoryProcessor
→ BeginnerProcessor
```

Порядок задаётся через `Priority`.

Если транспорт не проходит техосмотр, `TransportProcessor` возвращает `false`, и дальнейшая обработка останавливается.

Это сделано для того, чтобы создавая новый объект сразу делать все необходимые проверки и добавлять во все необходимые списки. Factory проходится по всем процессорам в порядке `Priority`, что позволяет добавить новый процессор без правки Factory.

## Технический осмотр

За техосмотр отвечает `ServiceCenter`.

Проверки вынесены в отдельные реализации `IInspectionRule`.

Сейчас используется правило:

```csharp
QualityInspectionRule
```

Проверяет, что качество quality > 9. Сделано для примера работы. Так как настоящие правила проверки качества могут быть различны, и их может быть много сделан интерфейс `IInspectionRule`. А `ServiceCenter` последовательно их применяет.

Транспорт проходит техосмотр, если выполняются все зарегистрированные правила.

Такой вариант был выбран, чтобы при добавлении новой проверки не пришлось переписывать `ServiceCenter`.

## Транспорт для новичков

У транспорта может быть значение:

```csharp
BeginnerEase
```

Если:

```text
BeginnerEase >= 6
```

то транспорт считается подходящим для новичка.

Эта проверка находится в `BeginnerService`, а `BeginnerProcessor` добавляет подходящие объекты в `BeginnerList`.

## Энергопотребление

Объекты, которые потребляют энергию, реализуют `IEnergyConsumer`.

`EnergyConsumptionService` считает суммарное ежедневное энергопотребление всех таких объектов, которые находятся в инвентаре.

## SOLID


### SRP

Классы разделены по обязанностям

**Сущности:** `Scooter`, `Helmet`, `ChargingStation` и интерфейсы, описывающие их возможности: `ITransport`, `IInvent`, `IEnergyConsumer`, `IBeginnerFriendly`.

**Техосмотр:** `ServiceCenter` выполняет проверку транспорта через набор `IInspectionRule`, например `QualityInspectionRule`.

**Хранение:** `Park`, `Inventory` и `BeginnerList` хранят соответствующие группы объектов.

**Обработка:** `TransportProcessor`, `InventoryProcessor` и `BeginnerProcessor` выполняют отдельные этапы обработки, а `EntityFactory` запускает их по цепочке.

**Сервисы и запуск:** `BeginnerService` и `EnergyConsumptionService` содержат отдельную бизнес-логику, `ConsoleApp` отвечает за работу с пользователем, а `DependencyInjection` и `Program` — за сборку и запуск приложения.

### OCP

Новое правило техосмотра можно добавить через новую реализацию `IInspectionRule`, не меняя сам `ServiceCenter`.

Аналогично новые сущности можно добавлять через существующие интерфейсы.

### LSP

Код работает через интерфейсы, поэтому реализации можно подменять.

Например, в тестах вместо настоящего `ServiceCenter` используется `FakeServiceCenter`.

### ISP

Интерфейсы разделены по назначению.

Например, `Helmet` реализует только `IInvent`, а `ChargingStation` — `IInvent` и `IEnergyConsumer`.

Им не приходится реализовывать интерфейсы, которые к ним не относятся.

### DIP

`TransportProcessor` зависит от `IServiceCenter`, а не от конкретного `ServiceCenter`.

`ServiceCenter`, в свою очередь, зависит от `IInspectionRule`.

Все зависимости передаются через конструкторы и собираются через DI-контейнер.

### Итог
Наиболее явно в проекте применены ISP и DIP. ISP виден в разделении интерфейсов `ITransport`, `IInvent`, `IEnergyConsumer`, `IBeginnerFriendly`, а DIP — в зависимостях `TransportProcessor` от `IServiceCenter` и `ServiceCenter` от `IInspectionRule`.

Сознательно ограничен DIP: для `Park`, `Inventory` и `BeginnerList` отдельные интерфейсы не создавались. Для небольшого проекта это упростило код и не создало проблем с расширением.

## Dependency Injection

Для DI используется:

```text
Microsoft.Extensions.DependencyInjection
```

`Park`, `Inventory` и `BeginnerList` зарегистрированы как `Singleton`, чтобы все части программы работали с одними и теми же экземплярами.

Через DI также регистрируются:

- `ServiceCenter`;
- правила техосмотра;
- processor;
- `EntityFactory`;
- консольное приложение.

## Тесты

Тесты написаны на xUnit.

Проверяются:

- правило для новичков;
- расчёт энергопотребления;
- прохождение и непрохождение техосмотра;
- работа нескольких правил техосмотра;
- `TransportProcessor`;
- полный pipeline;
- остановка pipeline после неудачного техосмотра;
- работа с инвентарём;
- корректность DI-регистрации.

Для тестов `TransportProcessor` используется `FakeServiceCenter`, которому можно заранее задать результат:

```csharp
new FakeServiceCenter(true)
```

или:

```csharp
new FakeServiceCenter(false)
```

Это позволяет тестировать `TransportProcessor` отдельно от реальной логики техосмотра.

## Покрытие

Основная бизнес-логика покрывается тестами.

`ConsoleApp` и `ConsoleInput` исключены из coverage через `ExcludeFromCodeCoverage`, потому что они в основном отвечают за ввод и вывод в консоль.


## Расширение

Проект сделан так, чтобы основные сценарии можно было расширять без переписывания уже существующей логики.

Чтобы добавить новый тип транспорта, достаточно создать новую сущность и реализовать нужные интерфейсы, например `ITransport`, `IInvent`, `IEnergyConsumer` или `IBeginnerFriendly`.

Чтобы добавить новое правило техосмотра, нужно:

1. реализовать `IInspectionRule`;
2. зарегистрировать новую реализацию в DI.

`ServiceCenter` автоматически получит все зарегистрированные правила и проверит их.

Также можно расширять цепочку обработки объектов. Для этого нужно создать новый класс, реализующий `IEntityProcessor`, задать ему `Priority` и зарегистрировать его в DI.

`EntityFactory` при этом менять не потребуется, потому что он работает со всеми зарегистрированными `IEntityProcessor`.

Новые инвентарные объекты можно добавлять через реализацию `IInvent`, а новые энергопотребляющие объекты — через `IEnergyConsumer`.

