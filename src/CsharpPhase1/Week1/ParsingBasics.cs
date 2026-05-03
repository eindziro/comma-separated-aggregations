using System.Globalization;

namespace CsharpPhase1.Week1;

/// <summary>
/// Фаза 1, неделя 1: разбор строк и простые агрегаты (без предметной области).
/// </summary>
public static class ParsingBasics
{
    /// <summary>
    /// Сумма целых через запятую, например <c>"1, 2, 3"</c> → 6.
    /// Пустая или только пробелы → 0. Невалидный фрагмент → <see cref="FormatException"/>.
    /// </summary>
    public static long SumCommaSeparatedIntegers(string line)
    {
        ArgumentNullException.ThrowIfNull(line);

        var trimmed = line.Trim();
        if (trimmed.Length == 0)
            return 0;

        long sum = 0;
        foreach (var part in trimmed.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            if (!long.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                throw new FormatException($"Expected integer, got '{part}'.");

            sum += value;
        }

        return sum;
    }

    /// <summary>
    /// Минимум среди целых через запятую. Пустая строка, только пробелы или нет ни одного числа → <see cref="InvalidOperationException"/>.
    /// Невалидный фрагмент → <see cref="FormatException"/>.
    /// </summary>
    public static long MinCommaSeparatedIntegers(string line)
    {
        ArgumentNullException.ThrowIfNull(line);
        return ReduceCommaSeparatedIntegers(line, static (a, b) => a < b ? a : b);
    }

    /// <summary>
    /// Максимум среди целых через запятую. Условия ошибок — как у <see cref="MinCommaSeparatedIntegers"/>.
    /// </summary>
    public static long MaxCommaSeparatedIntegers(string line)
    {
        ArgumentNullException.ThrowIfNull(line);
        return ReduceCommaSeparatedIntegers(line, static (a, b) => a > b ? a : b);
    }

    private static long ReduceCommaSeparatedIntegers(string line, Func<long, long, long> combine)
    {
        var trimmed = line.Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("At least one integer is required.");

        long? acc = null;
        foreach (var part in trimmed.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            if (!long.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                throw new FormatException($"Expected integer, got '{part}'.");

            acc = acc is null ? value : combine(acc.Value, value);
        }

        if (acc is null)
            throw new InvalidOperationException("At least one integer is required.");

        return acc.Value;
    }

    /// <summary>
    /// То же, что <see cref="SumCommaSeparatedIntegers"/>, но без исключений: false при null или неверном формате.
    /// </summary>
    public static bool TrySumCommaSeparatedIntegers(string line, out long sum)
    {
        sum = 0;
        if (line is null)
            return false;

        try
        {
            sum = SumCommaSeparatedIntegers(line);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static double AverageCommaSeparatedIntegers(string line)
    {
        ArgumentNullException.ThrowIfNull(line);

        var trimmed = line.Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("At least one integer is required.");

        int count = 0;
        long sum = 0;

        foreach (var part in trimmed.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            if (!long.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                throw new FormatException($"Expected integer, got '{part}'.");

            sum += value;
            count++;
        }

        if (count == 0)
            throw new InvalidOperationException("At least one integer is required.");

        return (double)sum / count;
    }
}
