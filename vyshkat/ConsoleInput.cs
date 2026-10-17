using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public class ConsoleInput
{
    public string ReadString(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
                return value;

            Console.WriteLine("Значение не может быть пустым.");
        }
    }

    public int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out var value))
                return value;

            Console.WriteLine("Введите целое число.");
        }
    }

    public int ReadIntInRange(
        string message,
        int min,
        int max)
    {
        while (true)
        {
            var value = ReadInt(message);

            if (value >= min && value <= max)
                return value;

            Console.WriteLine(
                $"Введите число от {min} до {max}.");
        }
    }

    public double ReadDouble(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (double.TryParse(
                    Console.ReadLine(),
                    out var value))
            {
                return value;
            }

            Console.WriteLine("Введите число.");
        }
    }
}