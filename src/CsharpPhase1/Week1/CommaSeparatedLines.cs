using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpPhase1.Week1;



/// <summary>

/// Построчная обработка текста: каждая строка — формат «числа через запятую», как в <see cref="ParsingBasics"/>.

/// Работа через <see cref="TextReader"/>, чтобы в тестах подставлять <see cref="StringReader"/> без файлов.

/// </summary>

public static class CommaSeparatedLines
{
    public static long TotalSumFromFile(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        using var reader = File.OpenText(path);
        return TotalSumFromAllLines(reader);
    }

    /// <summary>

    /// Лениво читает строки из <paramref name="reader"/> и для каждой возвращает сумму целых (правила — как у <see cref="ParsingBasics.SumCommaSeparatedIntegers"/>).

    /// </summary>

    public static IEnumerable<long> EnumerateLineSums(TextReader reader)

    {

        ArgumentNullException.ThrowIfNull(reader);



        while (reader.ReadLine() is { } line)

            yield return ParsingBasics.SumCommaSeparatedIntegers(line);

    }



    /// <summary>

    /// Сумма всех построчных сумм (пустой поток строк → 0).

    /// </summary>

    public static long TotalSumFromAllLines(TextReader reader)

    {

        long total = 0;

        foreach (var lineSum in EnumerateLineSums(reader))

            total += lineSum;



        return total;

    }

    /// <summary>
    /// Асинхронная версия <see cref="TotalSumFromAllLines"/>: читает строки из <paramref name="reader"/> и суммирует построчные суммы.
    /// </summary>
    public static async Task<long> TotalSumFromAllLinesAsync(TextReader reader, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(reader);

        long total = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null)
                break;

            total += ParsingBasics.SumCommaSeparatedIntegers(line);
        }

        return total;
    }

    public static async Task<long> TotalSumFromFileAsync(string path, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(path);

        await using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true);

        using var reader = new StreamReader(stream);
        return await TotalSumFromAllLinesAsync(reader, cancellationToken);
    }

}


